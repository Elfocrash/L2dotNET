using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using L2dotNET.DataContracts;
using L2dotNET.Network.loginauth.send;
using L2dotNET.Utility;
using L2dotNET.World;
using NLog;

namespace L2dotNET.Network.loginauth
{
    public class AuthThread
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public bool IsConnected { get; private set; }
        public int RandomPingKey { get; set; }

        private readonly Config.Config _config;
        private readonly GamePacketHandlerAuth _gamePacketHandlerAuth;
        private TcpClient _authServerConnection;
        private NetworkStream _networkStream;
        private byte[] _buffer;
        
        public AuthThread(Config.Config config)
        {
            _config = config;
        }

        public void Initialise()
        {
            IsConnected = false;
            try
            {
                _authServerConnection = new TcpClient(_config.ServerConfig.AuthHost, _config.ServerConfig.AuthPort);
                _networkStream = _authServerConnection.GetStream();
            }
            catch (SocketException ex)
            {
                Log.Error($"Socket Error: '{ex.SocketErrorCode}'. Message: '{ex.Message}' (Error Code: '{ex.NativeErrorCode}')");
                Log.Warn("Login server is not responding. Retrying in 5 seconds...");

                Task.Delay(5000).ContinueWith(x => Initialise());
                
                return;
            }

            IsConnected = true;

            SendPacket(new LoginAuth(_config));
            SendPacket(new LoginServPing(this));
            Read();
        }

        public void Read()
        {
            try
            {
                _buffer = new byte[2];
                _networkStream.BeginRead(_buffer, 0, 2, OnReceiveCallbackStatic, null);
            }
            catch (Exception e)
            {
                Log.Error($"{e.Message}");
                Termination();
            }
        }

        private void OnReceiveCallbackStatic(IAsyncResult result)
        {
            try
            {
                int rs = _networkStream.EndRead(result);
                if (rs <= 0)
                {
                    return;
                }

                short length = BitConverter.ToInt16(_buffer, 0);
                _buffer = new byte[length];
                _networkStream.BeginRead(_buffer, 0, length, new AsyncCallback(OnReceiveCallback), result.AsyncState);
            }
            catch (Exception e)
            {
                Log.Error($"{e.Message}");
                Termination();
            }
        }

        private void OnReceiveCallback(IAsyncResult result)
        {
            _networkStream.EndRead(result);

            byte[] buff = new byte[_buffer.Length];
            _buffer.CopyTo(buff, 0);
            Packet packet = buff.ToPacket();

            _gamePacketHandlerAuth.HandlePacket(packet, this);

            new System.Threading.Thread(Read).Start();
        }

        private void Termination()
        {
            if (_paused)
            {
                return;
            }

            Log.Error("Reconnecting...");
            Initialise();
        }

        public void SendPacket(GameserverPacket pk)
        {
            pk.Write();

            List<byte> blist = new List<byte>();
            byte[] db = pk.ToByteArray();

            short len = (short)db.Length;
            blist.AddRange(BitConverter.GetBytes(len));
            blist.AddRange(db);

            _networkStream.Write(blist.ToArray(), 0, blist.Count);
            _networkStream.Flush();
        }

        private bool _paused;

        public void LoginFail(string code)
        {
            _paused = true;
            Log.Error($"{code}. Please check configuration, server paused.");
            try
            {
                _networkStream.Close();
                _authServerConnection.Close();
            }
            catch (Exception e)
            {
                Log.Error($"{e.Message}");
            }
        }

        public void LoginOk(string code)
        {
            Log.Info($"{code}");
        }

        public void SetInGameAccount(string account, bool status = false)
        {
            SendPacket(new AccountInGame(account, status));
        }

        public void UpdatePlayersOnline()
        {
            short cnt = (short)L2World.GetPlayers().Count;
            SendPacket(new PlayerCount(cnt));
        }

        private readonly SortedList<string, AccountContract> _awaitingAccounts = new SortedList<string, AccountContract>();

        public AuthThread(GamePacketHandlerAuth gamePacketHandlerAuth, Config.Config config)
        {
            _gamePacketHandlerAuth = gamePacketHandlerAuth;
            _config = config;
        }

        public void AwaitAccount(AccountContract ta)
        {
            if (_awaitingAccounts.ContainsKey(ta.Login))
            {
                _awaitingAccounts.Remove(ta.Login);
            }

            _awaitingAccounts.Add(ta.Login, ta);
        }

        public AccountContract GetTa(string p)
        {
            if (!_awaitingAccounts.ContainsKey(p))
            {
                return null;
            }

            AccountContract ta = _awaitingAccounts[p];
            _awaitingAccounts.Remove(p);
            return ta;
        }
    }
}