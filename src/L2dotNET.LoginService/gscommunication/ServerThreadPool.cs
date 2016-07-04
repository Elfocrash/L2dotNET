using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using log4net;
using L2dotNET.LoginService.Model;
using L2dotNET.LoginService.Network;
using L2dotNET.Models;
using L2dotNET.Services.Contracts;
using Ninject;

namespace L2dotNET.LoginService.GSCommunication
{
    public class ServerThreadPool
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ServerThreadPool));

        [Inject]
        public IServerService ServerService
        {
            get { return LoginServer.Kernel.Get<IServerService>(); }
        }

        private static volatile ServerThreadPool _instance;
        private static readonly object SyncRoot = new object();

        public List<L2Server> Servers = new List<L2Server>();

        public static ServerThreadPool Instance
        {
            get
            {
                if (_instance == null)
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
            List<ServerModel> serverModels = ServerService.GetServerList();

            foreach (ServerModel curServ in serverModels)
            {
                L2Server server = new L2Server();
                server.Id = (byte)curServ.Id;
                server.Info = curServ.Name;
                server.Code = curServ.Code;
                Servers.Add(server);
            }

            Log.Info($"GameServerThread: loaded {Servers.Count} servers");
        }

        public L2Server Get(short serverId)
        {
            return Servers.FirstOrDefault(s => s.Id == serverId);
        }

        protected TcpListener Listener;

        public void Start()
        {
            Listener = new TcpListener(IPAddress.Parse(Config.Config.Instance.ServerConfig.Host), Config.Config.Instance.ServerConfig.GsPort);
            Listener.Start();
            Log.Info($"Auth server listening gameservers at {Config.Config.Instance.ServerConfig.Host}:{Config.Config.Instance.ServerConfig.GsPort}");
            while (true)
                VerifyClient(Listener.AcceptTcpClient());
        }

        private void VerifyClient(TcpClient client)
        {
            ServerThread st = new ServerThread();
            st.ReadData(client, this);
        }

        public void Shutdown(byte id)
        {
            foreach (L2Server s in Servers.Where(s => s.Id == id))
            {
                if (s.Thread != null)
                    s.Thread.Stop();

                s.Thread = null;
                Log.Warn($"ServerThread: #{id} shutted down");
                break;
            }
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
            foreach (L2Server srv in Servers.Where(srv => (srv.Id == serverId) && (srv.Thread != null)))
            {
                srv.Thread.SendPlayer(client, time);
                break;
            }
        }
    }
}