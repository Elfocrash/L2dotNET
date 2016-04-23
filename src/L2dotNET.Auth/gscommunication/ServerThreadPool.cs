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

namespace L2dotNET.Auth.gscommunication
{
    public class ServerThreadPool
    {
        [Inject]
        public IServerService serverService { get { return LoginServer.Kernel.Get<IServerService>(); } }

        private static ServerThreadPool gsc = new ServerThreadPool();
        public static ServerThreadPool getInstance()
        {
            return gsc;
        }

        public List<L2Server> servers = new List<L2Server>();

        public ServerThreadPool()
        {
            List<ServerModel> serverModels = serverService.GetServerList();

            foreach(var curServ in serverModels)
            {
                L2Server server = new L2Server();
                server.id = (byte)curServ.Id;
                server.info = curServ.Name;
                server.code = curServ.Code;
                servers.Add(server);
            }

            CLogger.info("GameServerThread: loaded " + servers.Count+" servers");
            
        }

        public L2Server get(short serverId)
        {
            foreach (L2Server s in servers)
                if (s.id == serverId)
                    return s;

            return null;
        }

        protected TcpListener listener;

        public void start()
        {
            listener = new TcpListener(IPAddress.Parse(Cfg.SERVER_HOST), Cfg.SERVER_PORT_GS);
            listener.Start();
            CLogger.extra_info("Auth server listening gameservers at " + Cfg.SERVER_HOST + ":" + Cfg.SERVER_PORT_GS);
            while (true)
            {
                verifyClient(listener.AcceptTcpClient());
            }
        }

        private void verifyClient(TcpClient client)
        {
            ServerThread st = new ServerThread();
            st.readData(client, this);
        }

        public void shutdown(byte id)
        {
            foreach (L2Server s in servers)
                if (s.id == id)
                {
                    if(s.thread != null)
                        s.thread.stop();

                    s.thread = null;
                    CLogger.warning("ServerThread: #"+id+" shutted down");
                    break;
                }
        }

        public bool LoggedAlready(string account)
        {
            foreach (L2Server srv in servers)
            {
                if (srv.thread != null)
                    if (srv.thread.LoggedAlready(account))
                    {
                        srv.thread.KickAccount(account);
                        return true;
                    }
            }

            return false;
        }

        public void SendPlayer(byte serverId, LoginClient client, string time)
        {
            foreach (L2Server srv in servers)
                if (srv.id == serverId && srv.thread != null)
                {
                    srv.thread.SendPlayer(client, time);
                    break;
                }

        }
    }
}
