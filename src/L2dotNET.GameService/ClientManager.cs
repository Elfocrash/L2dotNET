using System;
using System.Collections.Generic;
using System.Net.Sockets;
using log4net;
using L2dotNET.GameService.Network;

namespace L2dotNET.GameService
{
    class ClientManager
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ClientManager));

        private static volatile ClientManager instance;
        private static readonly object syncRoot = new object();

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

        protected SortedList<string, DateTime> Flood = new SortedList<string, DateTime>();
        protected NetworkBlock Banned;

        public SortedList<string, GameClient> Clients = new SortedList<string, GameClient>();

        public void AddClient(TcpClient client)
        {
            if (Banned == null)
            {
                Banned = NetworkBlock.Instance;
            }

            string ip = client.Client.RemoteEndPoint.ToString().Split(':')[0];

            lock (Flood)
            {
                if (Flood.ContainsKey(ip))
                {
                    if (Flood[ip].CompareTo(DateTime.Now) == 1)
                    {
                        log.Warn($"Active flooder: {ip}");
                        client.Close();
                        return;
                    }

                    lock (Flood)
                    {
                        Flood.Remove(ip);
                    }
                }
            }

            Flood.Add(ip, DateTime.Now.AddMilliseconds(3000));

            if (!Banned.Allowed(ip))
            {
                client.Close();
                log.Error($"NetworkBlock: connection attemp failed. {ip} banned.");
                return;
            }

            GameClient gc = new GameClient(client);

            lock (Clients)
            {
                Clients.Add(gc.Address.ToString(), gc);
            }
            log.Info($"NetController: {Clients.Count} active connections");
        }

        public void Terminate(string sock)
        {
            lock (Clients)
            {
                Clients.Remove(sock);
            }

            log.Info($"NetController: {Clients.Count} active connections");
        }
    }
}