using L2dotNET.model.player;
using L2dotNET.Network.serverpackets;
using L2dotNET.tools;

namespace L2dotNET.Network.clientpackets
{
    class RequestStartTrade : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _targetId;

        public RequestStartTrade(Packet packet, GameClient client)
        {
            _client = client;
            _targetId = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            if (player.TradeState != 0)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.AlreadyTrading);
                player.SendActionFailed();
                return;
            }

            if (player.ObjId == _targetId)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.CannotUseOnYourself);
                player.SendActionFailed();
                return;
            }

            if (player.EnchantState != 0)
            {
                player.SendActionFailed();
                return;
            }

            if (!(player.Target is L2Player))
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.TargetIsIncorrect);
                player.SendActionFailed();
                return;
            }

            L2Player target = (L2Player)player.Target;
            if (target.TradeState != 0)
            {
                player.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.S1AlreadyTrading).AddPlayerName(target.Name));
                player.SendActionFailed();
                return;
            }

            if (target.PartyState == 1)
            {
                player.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.S1IsBusyTryLater).AddPlayerName(target.Name));
                player.SendActionFailed();
                return;
            }

            if (!Calcs.CheckIfInRange(150, player, target, true))
            {
                player.SendActionFailed();
                return;
            }

            player.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.RequestS1ForTrade).AddPlayerName(target.Name));
            target.Requester = player;
            player.Requester = target;
            target.SendPacket(new SendTradeRequest(player.ObjId));
            target.TradeState = 2; // жмакает ответ
            player.TradeState = 1; // запросил
        }
    }
}