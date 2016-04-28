using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Sockets;
using L2dotNET.Auth.data;
using MySql.Data.MySqlClient;
using System;
using Ninject;
using L2dotNET.Services.Contracts;
using L2dotNET.Models;
using log4net;

namespace L2dotNET.Auth.gscommunication
{
    public class ServerThreadPool
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ServerThreadPool));

        [Inject]
        public IServerService serverService { get { return LoginServer.Kernel.Get<IServerService>(); } }

        private static volatile ServerThreadPool instance;
        private static object syncRoot = new object();

        public List<L2Server> servers = new List<L2Server>();

        public static ServerThreadPool Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new ServerThreadPool();
                        }
                    }
                }

                return instance;
            }
        }

        public ServerThreadPool()
        {
                       
        }

        public void Initialize()
        {
            List<ServerModel> serverModels = serverService.GetServerList();

            foreach (var curServ in serverModels)
            {
                L2Server server = new L2Server();
                server.Id = (byte)curServ.Id;
                server.Info = curServ.Name;
                server.Code = curServ.Code;
                servers.Add(server);
            }

            log.Info("GameServerThread: loaded " + servers.Count + " servers");
        }

        public L2Server Get(short serverId)
        {
            foreach (L2Server s in servers)
                if (s.Id == serverId)
                    return s;

            return null;
        }

        protected TcpListener listener;

        public void Start()
        {
            listener = new TcpListener(IPAddress.Parse(Cfg.SERVER_HOST), Cfg.SERVER_PORT_GS);
            listener.Start();
            log.Info("Auth server listening gameservers at " + Cfg.SERVER_HOST + ":" + Cfg.SERVER_PORT_GS);
            while (true)
            {
                VerifyClient(listener.AcceptTcpClient());
            }
        }

        private void VerifyClient(TcpClient client)
        {
            ServerThread st = new ServerThread();
            st.ReadData(client, this);
        }

        public void Shutdown(byte id)
        {
            foreach (L2Server s in servers)
                if (s.Id == id)
                {
                    if(s.Thread != null)
                        s.Thread.Stop();

                    s.Thread = null;
                    log.Warn($"ServerThread: #{id} shutted down");
                    break;
                }
        }

        public bool LoggedAlready(string account)
        {
            foreach (L2Server srv in servers)
            {
                if (srv.Thread != null)
                    if (srv.Thread.LoggedAlready(account))
                    {
                        srv.Thread.KickAccount(account);
                        return true;
                    }
            }

            return false;
        }

        public void SendPlayer(byte serverId, LoginClient client, string time)
        {
            foreach (L2Server srv in servers)
                if (srv.Id == serverId && srv.Thread != null)
                {
                    srv.Thread.SendPlayer(client, time);
                    break;
                }

        }
    }
}
