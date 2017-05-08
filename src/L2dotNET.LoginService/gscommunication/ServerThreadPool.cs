using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using log4net;
using L2dotNET.LoginService.Model;
using L2dotNET.LoginService.Network;
using L2dotNET.Services.Contracts;
using Ninject;

namespace L2dotNET.LoginService.GSCommunication
{
    public class ServerThreadPool
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ServerThreadPool));

        [Inject]
        public IServerService ServerService => LoginServer.Kernel.Get<IServerService>();

        private static volatile ServerThreadPool _instance;
        private static readonly object SyncRoot = new object();
        private TcpListener _listener;

        public List<L2Server> Servers = new List<L2Server>();

        public static ServerThreadPool Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new ServerThreadPool();
                }

                return _instance;
            }
        }

        public void Initialize()
        {
            Servers.AddRange(ServerService.GetServerList().Select(curServ => new L2Server
            {
                Id = (byte)curServ.Id,
                Info = curServ.Name,
                Code = curServ.Code
            }).ToList());

            Log.Info($"GameServerThread: loaded {Servers.Count} servers");
        }

        public L2Server Get(short serverId)
        {
            return Servers.FirstOrDefault(s => s.Id == serverId);
        }

        protected TcpListener Listener;

        public void Start()
        {
            //Listener = new TcpListener(IPAddress.Parse(Config.Config.Instance.ServerConfig.Host), Config.Config.Instance.ServerConfig.GsPort);
            //Listener.Start();
            //Log.Info($"Auth server listening gameservers at {Config.Config.Instance.ServerConfig.Host}:{Config.Config.Instance.ServerConfig.GsPort}");
            //while (true)
            //    VerifyClient(Listener.AcceptTcpClient());

            _listener = new TcpListener(IPAddress.Parse(Config.Config.Instance.ServerConfig.Host), Config.Config.Instance.ServerConfig.GsPort);

            try
            {
                _listener.Start();
                Log.Info($"Auth server listening gameservers at { Config.Config.Instance.ServerConfig.Host}:{Config.Config.Instance.ServerConfig.GsPort}");
            }
            catch (SocketException ex)
            {
                Log.Error($"Socket Error: '{ex.SocketErrorCode}'. Message: '{ex.Message}' (Error Code: '{ex.NativeErrorCode}')");
                Log.Info("Press ENTER to exit...");
                Console.Read();
                Environment.Exit(0);
            }

            WaitForClients();
        }

        private void WaitForClients()
        {
            _listener.BeginAcceptTcpClient(OnClientConnected, null);
        }

        private void OnClientConnected(IAsyncResult asyncResult)
        {
            TcpClient clientSocket = _listener.EndAcceptTcpClient(asyncResult);

            Log.Info($"Received connection request from: {clientSocket.Client.RemoteEndPoint}");

            VerifyClient(clientSocket);

            WaitForClients();
        }

        private void VerifyClient(TcpClient clientSocket)
        {
            ServerThread st = new ServerThread();
            st.ReadData(clientSocket, this);
        }

        public void Shutdown(byte id)
        {
            L2Server server = Servers.FirstOrDefault(s => s.Id == id);

            if (server == null)
                return;

            server.Thread?.Stop();
            server.Thread = null;
            Log.Warn($"ServerThread: #{id} shutted down");
        }

        public bool LoggedAlready(string account)
        {
            foreach (L2Server srv in Servers.Where(srv => (srv.Thread != null) && srv.Thread.LoggedAlready(account)))
            {
                srv.Thread.KickAccount(account);
                return true;
            }

            return false;
        }

        public void SendPlayer(byte serverId, LoginClient client, string time)
        {
            L2Server server = Servers.FirstOrDefault(srv => (srv.Id == serverId) && (srv.Thread != null));
            server?.Thread.SendPlayer(client, time);
        }
    }
}