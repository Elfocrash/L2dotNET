using System;
using System.Collections.Generic;
using System.Net.Sockets;
using log4net;
using L2dotNET.Models;
using L2dotNET.Network.loginauth.send;
using L2dotNET.Utility;
using L2dotNET.world;

namespace L2dotNET.Network.loginauth
{
    public class AuthThread
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AuthThread));
        private static volatile AuthThread _instance;
        private static readonly object SyncRoot = new object();

        public static AuthThread Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new AuthThread();
                }

                return _instance;
            }
        }

        protected TcpClient Lclient;
        protected NetworkStream Nstream;
        protected System.Timers.Timer Ltimer;
        public bool IsConnected;
        private byte[] _buffer;

        public string Version = "rcs #216";
        public int Build = 0;

        public void Initialize()
        {
            IsConnected = false;
            try
            {
                Lclient = new TcpClient(Config.Config.Instance.ServerConfig.AuthHost, Config.Config.Instance.ServerConfig.AuthPort);
                Nstream = Lclient.GetStream();
            }
            catch (SocketException)
            {
                Log.Warn("Login server is not responding. Retrying");
                if (Ltimer == null)
                {
                    Ltimer = new System.Timers.Timer
                    {
                        Interval = 2000
                    };
                    Ltimer.Elapsed += ltimer_Elapsed;
                }

                if (!Ltimer.Enabled)
                    Ltimer.Enabled = true;

                return;
            }

            if ((Ltimer != null) && Ltimer.Enabled)
                Ltimer.Enabled = false;

            IsConnected = true;

            SendPacket(new LoginAuth());
            SendPacket(new LoginServPing(this));
            new System.Threading.Thread(Read).Start();
        }

        private void ltimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Initialize();
        }

        public void Read()
        {
            try
            {
                _buffer = new byte[2];
                Nstream.BeginRead(_buffer, 0, 2, OnReceiveCallbackStatic, null);
            }
            catch (Exception e)
            {
                Log.Error($"AuthThread: {e.Message}");
                Termination();
            }
        }

        private void OnReceiveCallbackStatic(IAsyncResult result)
        {
            try
            {
                int rs = Nstream.EndRead(result);
                if (rs <= 0)
                    return;

                short length = BitConverter.ToInt16(_buffer, 0);
                _buffer = new byte[length];
                Nstream.BeginRead(_buffer, 0, length, new AsyncCallback(OnReceiveCallback), result.AsyncState);
            }
            catch (Exception e)
            {
                Log.Error($"AuthThread: {e.Message}");
                Termination();
            }
        }

        private void OnReceiveCallback(IAsyncResult result)
        {
            Nstream.EndRead(result);

            byte[] buff = new byte[_buffer.Length];
            _buffer.CopyTo(buff, 0);
            Packet packet = buff.ToPacket();

            GamePacketHandlerAuth.HandlePacket(packet, this);

            new System.Threading.Thread(Read).Start();
        }

        private void Termination()
        {
            if (_paused)
                return;

            Log.Error("AuthThread: reconnecting...");
            Initialize();
        }

        public void SendPacket(GameserverPacket pk)
        {
            pk.Write();

            List<byte> blist = new List<byte>();
            byte[] db = pk.ToByteArray();

            short len = (short)db.Length;
            blist.AddRange(BitConverter.GetBytes(len));
            blist.AddRange(db);

            Nstream.Write(blist.ToArray(), 0, blist.Count);
            Nstream.Flush();
        }

        private bool _paused;

        public void LoginFail(string code)
        {
            _paused = true;
            Log.Error($"AuthThread: {code}. Please check configuration, server paused.");
            try
            {
                Nstream.Close();
                Lclient.Close();
            }
            catch (Exception e)
            {
                Log.Error($"AuthThread: {e.Message}");
            }
        }

        public void LoginOk(string code)
        {
            Log.Info($"AuthThread: {code}");
        }

        public void SetInGameAccount(string account, bool status = false)
        {
            SendPacket(new AccountInGame(account, status));
        }

        public void UpdatePlayersOnline()
        {
            short cnt = (short)L2World.Instance.GetPlayers().Count;
            SendPacket(new PlayerCount(cnt));
        }

        private readonly SortedList<string, AccountModel> _awaitingAccounts = new SortedList<string, AccountModel>();

        public void AwaitAccount(AccountModel ta)
        {
            if (_awaitingAccounts.ContainsKey(ta.Login))
                _awaitingAccounts.Remove(ta.Login);

            _awaitingAccounts.Add(ta.Login, ta);
        }

        public AccountModel GetTa(string p)
        {
            if (!_awaitingAccounts.ContainsKey(p))
                return null;

            AccountModel ta = _awaitingAccounts[p];
            _awaitingAccounts.Remove(p);
            return ta;
        }
    }
}