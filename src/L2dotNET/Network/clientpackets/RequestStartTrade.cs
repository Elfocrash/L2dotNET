﻿using System;
using System.Threading.Tasks;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;
using L2dotNET.Tools;

namespace L2dotNET.Network.clientpackets
{
    class RequestStartTrade : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _targetId;

        public RequestStartTrade(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _targetId = packet.ReadInt();
        }

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                L2Player player = _client.CurrentPlayer;

                if (player.TradeState != 0)
                {
                    player.SendSystemMessage(SystemMessage.SystemMessageId.AlreadyTrading);
                    player.SendActionFailedAsync();
                    return;
                }

                if (player.CharacterId == _targetId)
                {
                    player.SendSystemMessage(SystemMessage.SystemMessageId.CannotUseOnYourself);
                    player.SendActionFailedAsync();
                    return;
                }

                if (player.EnchantState != 0)
                {
                    player.SendActionFailedAsync();
                    return;
                }

                if (!(player.Target is L2Player))
                {
                    player.SendSystemMessage(SystemMessage.SystemMessageId.TargetIsIncorrect);
                    player.SendActionFailedAsync();
                    return;
                }

                L2Player target = (L2Player)player.Target;
                if (target.TradeState != 0)
                {
                    player.SendPacketAsync(new SystemMessage(SystemMessage.SystemMessageId.S1AlreadyTrading).AddPlayerName(target.Name));
                    player.SendActionFailedAsync();
                    return;
                }

                if (target.PartyState == 1)
                {
                    player.SendPacketAsync(new SystemMessage(SystemMessage.SystemMessageId.S1IsBusyTryLater).AddPlayerName(target.Name));
                    player.SendActionFailedAsync();
                    return;
                }

                if (!Calcs.CheckIfInRange(150, player, target, true))
                {
                    player.SendActionFailedAsync();
                    return;
                }

                player.SendPacketAsync(new SystemMessage(SystemMessage.SystemMessageId.RequestS1ForTrade).AddPlayerName(target.Name));
                target.Requester = player;
                player.Requester = target;
                target.SendPacketAsync(new SendTradeRequest(player.CharacterId));
                target.TradeState = 2; // жмакает ответ
                player.TradeState = 1; // запросил
            });
        }
    }
}