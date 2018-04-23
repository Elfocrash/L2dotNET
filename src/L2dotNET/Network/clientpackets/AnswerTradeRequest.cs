using System;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;
using L2dotNET.Tools;

namespace L2dotNET.Network.clientpackets
{
    class AnswerTradeRequest : PacketBase
    {
        private readonly GameClient _client;
        private int _response;

        public AnswerTradeRequest(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _response = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            if (player.TradeState != 2)
            {
                player.SendMessage("Stupid.");
                _response = 0;
            }

            if (player.Requester == null)
            {
                player.SendMessage("Your trade requestor has logged off.");
                player.SendActionFailed();
                player.TradeState = 0;
                return;
            }

            if ((_response != 0) && (player.Requester.TradeState != 1))
                _response = 0;

            if ((_response != 0) && (player.EnchantState != 0))
                _response = 0;

            if ((_response != 0) && !Calcs.CheckIfInRange(150, player, player.Requester, true))
                _response = 0;

            switch (_response)
            {
                case 0:
                    player.TradeState = 0;
                    player.Requester.TradeState = 0;
                    player.Requester.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.S1DeniedTradeRequest).AddPlayerName(player.Name));
                    player.Requester.Requester = null;
                    player.Requester = null;
                    break;
                case 1:
                    player.Requester.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.BeginTradeWithS1).AddPlayerName(player.Name));
                    player.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.BeginTradeWithS1).AddPlayerName(player.Requester.Name));
                    player.TradeState = 3;
                    player.Requester.TradeState = 3;
                    player.SendPacket(new TradeStart(player));
                    player.Requester.SendPacket(new TradeStart(player.Requester));
                    break;
            }
        }
    }
}