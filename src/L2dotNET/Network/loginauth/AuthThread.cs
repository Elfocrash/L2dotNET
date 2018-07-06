using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using L2dotNET.DataContracts;
using L2dotNET.Logging.Abstraction;
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

        private readonly Dictionary<string, Tuple<AccountContract, SessionKey, DateTime>> _awaitingAccounts;

        public AuthThread(GamePacketHandlerAuth gamePacketHandlerAuth, Config.Config config)
        {
            _gamePacketHandlerAuth = gamePacketHandlerAuth;
            _config = config;
            _awaitingAccounts = new Dictionary<string, Tuple<AccountContract, SessionKey, DateTime>>();
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

            Task.Factory.StartNew(Read);
        }

        public async void Read()
        {
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[2];
                    int bytesRead = await _networkStream.ReadAsync(buffer, 0, 2);

                    if (bytesRead != 2)
                    {
                        throw new Exception("Wrong packet");
                    }

                    short length = BitConverter.ToInt16(buffer, 0);

                    buffer = new byte[length];
                    bytesRead = await _networkStream.ReadAsync(buffer, 0, length);

                    if (bytesRead != length)
                    {
                        throw new Exception("Wrong packet");
                    }

                    Task.Factory.StartNew(() => _gamePacketHandlerAuth.HandlePacket(buffer.ToPacket(), this));
                }
            }
            catch (Exception e)
            {
                Log.Error($"{e.Message}");
                Reconnect();
            }
        }
        
        private void Reconnect()
        {
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


        public void LoginFail(string code)
        {
            try
            {
                _networkStream.Close();
                _authServerConnection.Close();
            }
            catch (Exception e)
            {
                Log.Error($"{e.Message}");
            }

            Log.Halt($"Please check configuration. Error code: {code}");
        }

        public void LoginOk(string code)
        {
            Log.Info($"Auth server successfully connected. {code}");
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

        public void AwaitAddAccount(AccountContract account, SessionKey key)
        {
            if (_awaitingAccounts.ContainsKey(account.Login))
            {
                _awaitingAccounts.Remove(account.Login);
            }

            _awaitingAccounts.Add(account.Login, new Tuple<AccountContract, SessionKey, DateTime>(account, key, DateTime.UtcNow));
        }

        public Tuple<AccountContract, SessionKey, DateTime> GetAwaitingAccount(string login)
        {
            if (!_awaitingAccounts.ContainsKey(login))
            {
                return null;
            }

            Tuple<AccountContract, SessionKey, DateTime> accountTuple = _awaitingAccounts[login];
            _awaitingAccounts.Remove(login);

            return accountTuple;
        }
    }
}