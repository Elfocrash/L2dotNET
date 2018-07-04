using System;
using System.Collections.Concurrent;
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

        private readonly ConcurrentDictionary<string, DateTime> _flood;
        private readonly ConcurrentDictionary<string, GameClient> _loggedClients;
        private readonly GamePacketHandler _gamePacketHandler;

        public ClientManager(ICharacterService characterService, GamePacketHandler gamePacketHandler)
        {
            CharacterService = characterService;
            _gamePacketHandler = gamePacketHandler;

            _flood = new ConcurrentDictionary<string, DateTime>();
            _loggedClients = new ConcurrentDictionary<string, GameClient>();
        }

        public void AddClient(TcpClient client)
        {
            string ip = client.Client.RemoteEndPoint.ToString().Split(':')[0];

            if (_flood.ContainsKey(ip))
            {
                if (_flood[ip].CompareTo(DateTime.UtcNow) == 1)
                {
                    Log.Warn($"Active flooder: {ip}");
                    client.Close();
                    return;
                }

                DateTime oldDate;
                _flood.TryRemove(ip, out oldDate);
            }

            _flood.AddOrUpdate(ip, DateTime.UtcNow.AddMilliseconds(3000), (a, b) => DateTime.UtcNow.AddMilliseconds(3000));

            if (!NetworkBlock.Instance.Allowed(ip))
            {
                client.Close();
                Log.Error($"NetworkBlock: connection attemp failed. IP: {ip} banned.");
                return;
            }

            GameClient gameClient = new GameClient(CharacterService, this, client, _gamePacketHandler);

            _loggedClients.TryAdd(gameClient.Address.ToString(), gameClient);

            Log.Info($"{_loggedClients.Count} active connections");
        }

        public void Terminate(string sock)
        {
            GameClient o;
            _loggedClients.TryRemove(sock, out o);

            Log.Info($"{_loggedClients.Count} active connections");
        }
    }
}