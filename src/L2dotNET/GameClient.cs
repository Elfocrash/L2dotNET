using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using L2dotNET.DataContracts;
using L2dotNET.Encryption;
using L2dotNET.Models.Player;
using L2dotNET.Network;
using L2dotNET.Network.serverpackets;
using L2dotNET.Services.Contracts;
using L2dotNET.Utility;
using L2dotNET.World;
using NLog;

namespace L2dotNET
{
    public class GameClient
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public EndPoint Address { get; }
        public TcpClient Client { get; }
        public NetworkStream Stream { get; }
        public byte[] BlowfishKey { get; set; }
        public ScrambledKeyPair ScrambledPair { get; set; }
        public L2Player CurrentPlayer { get; set; }
        public AccountContract Account { get; set; }
        public SessionKey SessionKey { get; set; }
        public string AccountType { get; set; }
        public string AccountTimeEnd { get; set; }
        public DateTime AccountTimeLogIn { get; set; }
        public bool IsDisconnected { get; set; }

        public List<L2Player> AccountCharacters { get; }

        private readonly ICharacterService _characterService;
        private readonly ClientManager _clientManager;
        private readonly GamePacketHandler _gamePacketHandler;
        private readonly GameCrypt _crypt;

        public GameClient(ICharacterService characterService, ClientManager clientManager, TcpClient tcpClient, GamePacketHandler gamePacketHandler)
        {
            Log.Info($"Connection from {tcpClient.Client.RemoteEndPoint}");

            Address = tcpClient.Client.RemoteEndPoint;
            Client = tcpClient;
            Stream = tcpClient.GetStream();
            AccountCharacters = new List<L2Player>();

            _crypt = new GameCrypt();
            _characterService = characterService;
            _gamePacketHandler = gamePacketHandler;
            _clientManager = clientManager;

            Task.Factory.StartNew(Read);
        }

        public byte[] EnableCrypt()
        {
            byte[] key = BlowFishKeygen.GetRandomKey();
            _crypt.SetKey(key);
            return key;
        }

        public async Task SendPacketAsync(GameserverPacket sbp)
        {
            if (IsDisconnected)
            {
                return;
            }

            sbp.Write();
            byte[] data = sbp.ToByteArray();
            _crypt.Encrypt(data);
            List<byte> bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes((short)(data.Length + 2)));
            bytes.AddRange(data);
            
            try
            {
                await Stream.WriteAsync(bytes.ToArray(), 0, bytes.Count);
                await Stream.FlushAsync();
            }
            catch
            {
                Log.Info($"Client {Account?.AccountId} terminated.");
                Disconnect();
            }
        }

        public void Disconnect()
        {
            Log.Info("termination");

            IsDisconnected = true;

            Stream.Close();
            Client.Close();

            if(CurrentPlayer?.Online == 1)
            {
                CurrentPlayer?.DeleteMe();
            }

            _clientManager.Disconnect(Address.ToString());
        }

        public async void Read()
        {
            try
            {
                while (true)
                {
                    if (IsDisconnected)
                    {
                        return;
                    }

                    byte[] _buffer = new byte[2];
                    int bytesRead = await Stream.ReadAsync(_buffer, 0, 2);

                    if (bytesRead != 2)
                    {
                        throw new Exception("Wrong packet");
                    }

                    short length = BitConverter.ToInt16(_buffer, 0);
                    _buffer = new byte[length - 2];

                    bytesRead = await Stream.ReadAsync(_buffer, 0, length - 2);

                    if (bytesRead != length - 2)
                    {
                        throw new Exception("Wrong packet");
                    }

                    _crypt.Decrypt(_buffer);

                    Task.Factory.StartNew(() => _gamePacketHandler.HandlePacket(_buffer.ToPacket(), this));
                }
            }
            catch(Exception e)
            {
                Log.Error(e);
                Disconnect();
            }
        }

        public async Task<L2Player> GetPlayer(int accountId, int charSlot)
        {
            L2Player playerContract = await _characterService.GetPlayerBySlotId(accountId, charSlot);
            L2Player player = L2World.GetPlayer(playerContract.ObjectId);
            return player;
        }

        public void DeleteCharacter(int charSlot)
        {
            AccountCharacters.RemoveAll(x => x.CharacterSlot == charSlot);
            
            for (int i = 0; i < AccountCharacters.Count; i++)
            {
                AccountCharacters[i].CharacterSlot = i;
            }
        }
    }
}