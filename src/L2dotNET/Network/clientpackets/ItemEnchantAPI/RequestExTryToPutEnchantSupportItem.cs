﻿using System;
using L2dotNET.Managers;
using L2dotNET.Models.Items;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets.ItemEnchantAPI
{
    class RequestExTryToPutEnchantSupportItem : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _aSSupportId;
        private readonly int _aSTargetId;

        public RequestExTryToPutEnchantSupportItem(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            packet.MoveOffset(2);
            _client = client;
            _aSSupportId = packet.ReadInt();
            _aSTargetId = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            if ((player.EnchantState != ItemEnchantManager.StateEnchantStart) || (player.EnchantItem.ObjId != _aSTargetId))
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.RegistrationOfEnhancementSpellbookHasFailed);
                player.SendActionFailed();
                return;
            }

            L2Item stone = player.GetItemByObjId(_aSSupportId);

            if (stone == null)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.RegistrationOfEnhancementSpellbookHasFailed);
                player.SendActionFailed();
                return;
            }

            ItemEnchantManager.GetInstance().TryPutStone(player, stone);
        }
    }
}