﻿using System;
using L2dotNET.Managers;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets.ItemEnchantAPI
{
    class RequestExCancelEnchantItem : PacketBase
    {
        private readonly GameClient _client;

        public RequestExCancelEnchantItem(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            packet.MoveOffset(2);
            _client = client;
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            player.EnchantScroll = null;

            switch (player.EnchantState)
            {
                case ItemEnchantManager.StateEnchantStart:
                    player.EnchantItem = null;
                    break;
            }

            player.EnchantState = 0;
            player.SendPacket(new EnchantResult(EnchantResultVal.CloseWindow));
        }
    }
}