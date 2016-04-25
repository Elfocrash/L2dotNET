using System;
using System.Collections.Generic;
using System.Net.Sockets;
using L2dotNET.Game.logger;
using L2dotNET.Game.network;

namespace L2dotNET.Game
{
    class ClientManager
    {
        private static volatile ClientManager instance;
        private static object syncRoot = new object();

        public static ClientManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new ClientManager();
                        }
                    }
                }

                return instance;
            }
        }

        public ClientManager()
        {

        }

        protected SortedList<string, DateTime> _flood = new SortedList<string, DateTime>();
        protected NetworkBlock _banned;

        public SortedList<string, GameClient> clients = new SortedList<string, GameClient>();

        public void addClient(TcpClient client)
        {
            if(_banned == null)
                _banned = NetworkBlock.Instance;

            string ip = client.Client.RemoteEndPoint.ToString().Split(':')[0];

            if (_flood.ContainsKey(ip))
            {
                if (_flood[ip].CompareTo(DateTime.Now) == 1)
                {
                    CLogger.warning("active flooder " + ip);
                    client.Close();
                    return;
                }
                else
                    lock (_flood)
                        _flood.Remove(ip);
            }

            _flood.Add(ip, DateTime.Now.AddMilliseconds(3000));

            if (!_banned.Allowed(ip))
            {
                client.Close();
                CLogger.error("NetworkBlock: connection attemp failed. "+ip+" banned.");
                return;
            }

            GameClient gc = new GameClient(client);

            clients.Add(gc._address.ToString(), gc);
            CLogger.extra_info("NetController: " + clients.Count + " active connections");
        }

        public void terminate(string sock)
        {
            lock (clients)
                clients.Remove(sock);

            CLogger.extra_info("NetController: " + clients.Count + " active connections");
        }
    }
}
