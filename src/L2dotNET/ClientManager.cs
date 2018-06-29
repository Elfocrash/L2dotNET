using System;
using System.Collections.Generic;
using System.Net.Sockets;
using L2dotNET.Logging.Abstraction;
using L2dotNET.Network;
using L2dotNET.Services.Contracts;

namespace L2dotNET
{
    public class ClientManager
    {
        private static readonly ILog log = LogProvider.GetCurrentClassLogger();

        private readonly ICharacterService CharacterService;
       
        protected SortedList<string, DateTime> Flood = new SortedList<string, DateTime>();
        protected NetworkBlock Banned;

        public SortedList<string, GameClient> Clients = new SortedList<string, GameClient>();
        private readonly GamePacketHandler _gamePacketHandler;

        public ClientManager(ICharacterService characterService, GamePacketHandler gamePacketHandler)
        {
            CharacterService = characterService;
            _gamePacketHandler = gamePacketHandler;
        }

        public void AddClient(TcpClient client)
        {
            if (Banned == null)
                Banned = NetworkBlock.Instance;

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
                        Flood.Remove(ip);
                }
            }

            Flood.Add(ip, DateTime.Now.AddMilliseconds(3000));

            if (!Banned.Allowed(ip))
            {
                client.Close();
                log.Error($"Connection attempt failed. {ip} banned.");
                return;
            }

            GameClient gc = new GameClient(CharacterService, this, client, _gamePacketHandler);

            lock (Clients)
                Clients.Add(gc.Address.ToString(), gc);
            log.Info($"{Clients.Count} active connections");
        }

        public void Terminate(string sock)
        {
            lock (Clients)
                Clients.Remove(sock);

            log.Info($"{Clients.Count} active connections");
        }
    }
}