using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using L2dotNET.Logging.Abstraction;
using L2dotNET.LoginService.Model;
using L2dotNET.LoginService.Network;
using L2dotNET.Services.Contracts;

namespace L2dotNET.LoginService.GSCommunication
{
    public class ServerThreadPool
    {
        private static readonly ILog Log = LogProvider.GetCurrentClassLogger();

        private readonly IServerService _serverService;
        private readonly Config.Config _config;
        private readonly PacketHandler _packetHandler;

        public ServerThreadPool(IServerService serverService, Config.Config config, PacketHandler packetHandler)
        {
            _serverService = serverService;
            _config = config;
            _packetHandler = packetHandler;
        }

        private TcpListener _listener;

        public List<L2Server> Servers = new List<L2Server>();
        
        public void Initialize()
        {
            Servers.AddRange(_serverService.GetServerList().Select(curServ => new L2Server
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

            _listener = new TcpListener(IPAddress.Parse(_config.ServerConfig.Host), _config.ServerConfig.GsPort);

            try
            {
                _listener.Start();
                Log.Info($"Auth server listening gameservers at {_config.ServerConfig.Host}:{_config.ServerConfig.GsPort}");
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
            ServerThread st = new ServerThread(_packetHandler);
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