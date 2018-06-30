﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
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
        private readonly ICharacterService CharacterService;
        private readonly ClientManager _clientManager;
        private readonly GamePacketHandler _gamePacketHandler;
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

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

        public GameClient(ICharacterService characterService, ClientManager clientManager, TcpClient tcpClient, GamePacketHandler gamePacketHandler)
        {
            CharacterService = characterService;
            Log.Info($"Connection from {tcpClient.Client.RemoteEndPoint}");
            Client = tcpClient;
            _gamePacketHandler = gamePacketHandler;
            _clientManager = clientManager;
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

        public async Task SendPacketAsync(GameserverPacket sbp)
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
                //    Log.Info($"{ s:X2 } ");
            }

            try
            {
                await Stream.WriteAsync(bytes.ToArray(), 0, bytes.Count);
                await Stream.FlushAsync();
            }
            catch
            {
                Log.Info($"Client {AccountName} terminated.");
                Termination();
            }
        }

        public void Termination()
        {
            Log.Info("termination");
            IsTerminated = true;
            Stream.Close();
            Client.Close();

            if(CurrentPlayer?.Online == 1)
                CurrentPlayer?.DeleteMe();

            //_accountChars.ForEach(p => p?.DeleteMe());
            //_accountChars.Clear();

            _clientManager.Terminate(Address.ToString());
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

        public async Task<L2Player> LoadPlayerInSlot(string accName, int charSlot)
        {
            L2Player player = await CharacterService.GetPlayerBySlotId(accName, charSlot);
            return player;
        }

        public async Task<L2Player> GetPlayer(string accName, int charSlot)
        {
            L2Player playerContract = await LoadPlayerInSlot(accName, charSlot);
            L2Player player = L2World.Instance.GetPlayer(playerContract.ObjId);
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

            _gamePacketHandler.HandlePacket(new Packet(1, buff), this);

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
        public SessionKey SessionKey { get; set; }
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