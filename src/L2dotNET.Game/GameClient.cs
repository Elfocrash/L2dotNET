using L2Crypt;
using L2dotNET.GameService.crypt;
using L2dotNET.GameService.network;
using L2dotNET.GameService.network.l2send;
using L2dotNET.GameService.world;
using L2dotNET.Models;
using L2dotNET.Services.Contracts;
using log4net;
using Ninject;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace L2dotNET.GameService
{
    public class GameClient
    {
        [Inject]
        public IPlayerService playerService { get { return GameServer.Kernel.Get<IPlayerService>(); } }

        private static readonly ILog log = LogManager.GetLogger(typeof(GameClient));

        public EndPoint _address;
        public TcpClient _client;
        public NetworkStream _stream;
        private byte[] _buffer;
        private GameCrypt _crypt;
        public byte[] _blowfishKey;
        public ScrambledKeyPair _scrambledPair;

        public L2Player CurrentPlayer;

        public List<L2Player> _accountChars = new List<L2Player>();
        public int Protocol;
        public bool IsTerminated;

        public long TrafficUp = 0, TrafficDown = 0;

        public GameClient(TcpClient tcpClient)
        {
            log.Info($"connection from { tcpClient.Client.RemoteEndPoint }");
            _client = tcpClient;
            _stream = tcpClient.GetStream();
            _address = tcpClient.Client.RemoteEndPoint;
            _crypt = new GameCrypt();
            new System.Threading.Thread(read).Start();
        }

        public byte[] enableCrypt()
        {
            byte[] key = BlowFishKeygen.getRandomKey();
            _crypt.setKey(key);
            return key;
        }

        public void sendPacket(GameServerNetworkPacket sbp)
        {
            if (IsTerminated)
                return;

            sbp.write();
            byte[] data = sbp.ToByteArray();
            _crypt.encrypt(data);
            List<byte> bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes((short)(data.Length + 2)));
            bytes.AddRange(data);
            TrafficDown += bytes.Count;

            if (sbp is CharacterSelectionInfo)
            {

                // byte[] st = ToByteArray();
                //foreach (byte s in data)
                //    log.Info($"{ s.ToString("x2") } ");
            }

            try
            {
                _stream.Write(bytes.ToArray(), 0, bytes.Count);
                //  _stream.Flush();
            }
            catch
            {
                log.Info($"client { AccountName } terminated.");
                termination();
            }
        }

        public void termination()
        {
            log.Info("termination");
            IsTerminated = true;
            _stream.Close();
            _client.Close();

            if (CurrentPlayer != null)
                CurrentPlayer.Termination();

            foreach (L2Player p in _accountChars)
                p.Termination();

            _accountChars.Clear();

            ClientManager.Instance.terminate(_address.ToString());
        }

        public void read()
        {
            if (IsTerminated)
                return;

            try
            {
                _buffer = new byte[2];
                _stream.BeginRead(_buffer, 0, 2, new AsyncCallback(OnReceiveCallbackStatic), null);
            }
            catch
            {
                termination();
            }
        }

        public PlayerModel LoadPlayerInSlot(string accName, int charSlot)
        {
            PlayerModel player = playerService.GetPlayerModelBySlotId(accName, charSlot);
            return player;
        }

        public L2Player GetPlayer(string accName, int charSlot)
        {
            PlayerModel playerModel = LoadPlayerInSlot(accName, charSlot);
            L2Player player = L2World.Instance.GetPlayer(playerModel.ObjectId);
            return player;
        }

        private void OnReceiveCallbackStatic(IAsyncResult result)
        {
            int rs = 0;
            try
            {
                rs = _stream.EndRead(result);
                if (rs > 0)
                {
                    short length = BitConverter.ToInt16(_buffer, 0);
                    _buffer = new byte[length - 2];
                    _stream.BeginRead(_buffer, 0, length - 2, new AsyncCallback(OnReceiveCallback), result.AsyncState);
                }
            }
            catch
            {
                termination();
            }
        }

        private void OnReceiveCallback(IAsyncResult result)
        {
            if (IsTerminated)
                return;

            _stream.EndRead(result);

            byte[] buff = new byte[_buffer.Length];
            _buffer.CopyTo(buff, 0);
            _crypt.decrypt(buff);
            TrafficUp += _buffer.Length;

            PacketHandler.handlePacket(this, buff);

            new System.Threading.Thread(read).Start();
        }

        public string AccountName { get; set; }
        public int SessionId { get; set; }
        public string AccountType { get; set; }
        public string AccountTimeEnd { get; set; }
        public DateTime AccountTimeLogIn { get; set; }
    }
}
