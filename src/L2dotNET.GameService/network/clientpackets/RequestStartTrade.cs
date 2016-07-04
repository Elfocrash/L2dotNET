using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.Tools;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestStartTrade : GameServerNetworkRequest
    {
        private int targetId;

        public RequestStartTrade(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            targetId = readD();
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            if (player.TradeState != 0)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.ALREADY_TRADING);
                player.SendActionFailed();
                return;
            }

            if (player.ObjId == targetId)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.CANNOT_USE_ON_YOURSELF);
                player.SendActionFailed();
                return;
            }

            if (player.EnchantState != 0)
            {
                player.SendActionFailed();
                return;
            }

            if ((player.CurrentTarget == null) || !(player.CurrentTarget is L2Player))
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.TARGET_IS_INCORRECT);
                player.SendActionFailed();
                return;
            }

            L2Player target = (L2Player)player.CurrentTarget;
            if (target.TradeState != 0)
            {
                player.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.S1_ALREADY_TRADING).AddPlayerName(target.Name));
                player.SendActionFailed();
                return;
            }

            if (target.PartyState == 1)
            {
                player.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.S1_IS_BUSY_TRY_LATER).AddPlayerName(target.Name));
                player.SendActionFailed();
                return;
            }

            if (!Calcs.checkIfInRange(150, player, target, true))
            {
                player.SendActionFailed();
                return;
            }

            player.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.REQUEST_S1_FOR_TRADE).AddPlayerName(target.Name));
            target.requester = player;
            player.requester = target;
            target.SendPacket(new SendTradeRequest(player.ObjId));
            target.TradeState = 2; // жмакает ответ
            player.TradeState = 1; // запросил
        }
    }
}