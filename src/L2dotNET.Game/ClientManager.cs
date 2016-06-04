using System;
using System.Collections.Generic;
using System.Net.Sockets;
using log4net;
using L2dotNET.GameService.network;

namespace L2dotNET.GameService
{
    class ClientManager
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ClientManager));

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
            if (_banned == null)
                _banned = NetworkBlock.Instance;

            string ip = client.Client.RemoteEndPoint.ToString().Split(':')[0];

            if (_flood.ContainsKey(ip))
            {
                if (_flood[ip].CompareTo(DateTime.Now) == 1)
                {
                    log.Warn($"Active flooder: { ip }");
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
                log.Error($"NetworkBlock: connection attemp failed. { ip } banned.");
                return;
            }

            GameClient gc = new GameClient(client);

            clients.Add(gc._address.ToString(), gc);
            log.Info($"NetController: { clients.Count } active connections");
        }

        public void terminate(string sock)
        {
            lock (clients)
                clients.Remove(sock);

            log.Info($"NetController: { clients.Count } active connections");
        }
    }
}
