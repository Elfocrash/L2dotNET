using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using log4net;
using L2dotNET.Encryption;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.World;
using L2dotNET.Models;
using L2dotNET.Network;
using L2dotNET.Services.Contracts;
using L2dotNET.Utility;
using Ninject;

namespace L2dotNET.GameService
{
    public class GameClient
    {
        [Inject]
        public IPlayerService PlayerService => GameServer.Kernel.Get<IPlayerService>();

        private static readonly ILog Log = LogManager.GetLogger(typeof(GameClient));

        public EndPoint Address;
        public TcpClient Client;
        public NetworkStream Stream;
        private byte[] _buffer;
        private readonly GameCrypt _crypt;
        public byte[] BlowfishKey;
        public ScrambledKeyPair ScrambledPair;

        public L2Player CurrentPlayer;

        private List<L2Player> _accountChars = new List<L2Player>();
        public int Protocol;
        public bool IsTerminated;

        public long TrafficUp,
                    TrafficDown;

        public GameClient(TcpClient tcpClient)
        {
            Log.Info($"connection from {tcpClient.Client.RemoteEndPoint}");
            Client = tcpClient;
            Stream = tcpClient.GetStream();
            Address = tcpClient.Client.RemoteEndPoint;
            _crypt = new GameCrypt();
            new System.Threading.Thread(Read).Start();
        }

        public byte[] EnableCrypt()
        {
            byte[] key = BlowFishKeygen.GetRandomKey();
            _crypt.SetKey(key);
            return key;
        }

        public void SendPacket(GameserverPacket sbp)
        {
            if (IsTerminated)
                return;

            sbp.Write();
            byte[] data = sbp.ToByteArray();
            _crypt.Encrypt(data);
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
                Stream.Write(bytes.ToArray(), 0, bytes.Count);
                //  _stream.Flush();
            }
            catch
            {
                Log.Info($"client {AccountName} terminated.");
                Termination();
            }
        }

        public void Termination()
        {
            Log.Info("termination");
            IsTerminated = true;
            Stream.Close();
            Client.Close();

            CurrentPlayer?.Termination();

            _accountChars.ForEach(p => p.Termination());
            _accountChars.Clear();

            ClientManager.Instance.Terminate(Address.ToString());
        }

        public void Read()
        {
            if (IsTerminated)
                return;

            try
            {
                _buffer = new byte[2];
                Stream.BeginRead(_buffer, 0, 2, OnReceiveCallbackStatic, null);
            }
            catch
            {
                Termination();
            }
        }

        public PlayerModel LoadPlayerInSlot(string accName, int charSlot)
        {
            PlayerModel player = PlayerService.GetPlayerModelBySlotId(accName, charSlot);
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
            try
            {
                int rs = Stream.EndRead(result);
                if (rs <= 0)
                    return;

                short length = BitConverter.ToInt16(_buffer, 0);
                _buffer = new byte[length - 2];
                Stream.BeginRead(_buffer, 0, length - 2, OnReceiveCallback, result.AsyncState);
            }
            catch
            {
                Termination();
            }
        }

        private void OnReceiveCallback(IAsyncResult result)
        {
            if (IsTerminated)
                return;

            Stream.EndRead(result);

            byte[] buff = new byte[_buffer.Length];
            _buffer.CopyTo(buff, 0);
            _crypt.Decrypt(buff);
            TrafficUp += _buffer.Length;

            PacketHandler.HandlePacket(new Packet(1, buff), this);

            new System.Threading.Thread(Read).Start();
        }

        public void RemoveAccountCharAndResetSlotIndex(int charSlot)
        {
            AccountChars = AccountChars.Where(filter => filter.CharSlot != charSlot).OrderBy(orderby => orderby.CharSlot).ToList();

            int slot = 0;
            AccountChars.ForEach(p =>
            {
                p.CharSlot = slot;
                slot++;
            });
        }

        public string AccountName { get; set; }
        public int SessionId { get; set; }
        public string AccountType { get; set; }
        public string AccountTimeEnd { get; set; }
        public DateTime AccountTimeLogIn { get; set; }

        public List<L2Player> AccountChars
        {
            get { return _accountChars ?? new List<L2Player>(); }
            set { _accountChars = value; }
        }
    }
}