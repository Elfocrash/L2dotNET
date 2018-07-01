using System;
using System.Collections.Generic;
using System.Net.Sockets;
using L2dotNET.Network;
using L2dotNET.Services.Contracts;
using NLog;

namespace L2dotNET
{
    public class ClientManager
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

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
                        Log.Warn($"Active flooder: {ip}");
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
                Log.Error($"Connection attempt failed. {ip} banned.");
                return;
            }

            GameClient gc = new GameClient(CharacterService, this, client, _gamePacketHandler);

            lock (Clients)
                Clients.Add(gc.Address.ToString(), gc);
            Log.Info($"{Clients.Count} active connections");
        }

        public void Terminate(string sock)
        {
            lock (Clients)
                Clients.Remove(sock);

            Log.Info($"{Clients.Count} active connections");
        }
    }
}