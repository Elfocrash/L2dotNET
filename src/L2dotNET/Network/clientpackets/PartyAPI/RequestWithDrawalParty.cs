﻿using System;
using System.Threading.Tasks;

namespace L2dotNET.Network.clientpackets.PartyAPI
{
    class RequestWithdrawalParty : PacketBase
    {
        private readonly GameClient _client;

        public RequestWithdrawalParty(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
        }

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                var player = _client.CurrentPlayer;

                if (player.Party == null)
                {
                    player.SendActionFailedAsync();
                    return;
                }

                player.Party.Leave(player);
            });
        }
    }
}