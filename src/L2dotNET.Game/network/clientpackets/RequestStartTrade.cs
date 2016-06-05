using L2dotNET.GameService.Model.player;
using L2dotNET.GameService.network.serverpackets;
using L2dotNET.GameService.tools;

namespace L2dotNET.GameService.network.clientpackets
{
    class RequestStartTrade : GameServerNetworkRequest
    {
        private int targetId;

        public RequestStartTrade(GameClient client, byte[] data)
        {
            base.makeme(client, data);
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
                player.sendSystemMessage(SystemMessage.SystemMessageId.ALREADY_TRADING);
                player.sendActionFailed();
                return;
            }

            if (player.ObjID == targetId)
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.CANNOT_USE_ON_YOURSELF);
                player.sendActionFailed();
                return;
            }

            if (player.EnchantState != 0)
            {
                player.sendActionFailed();
                return;
            }

            L2Player target;
            if (player.CurrentTarget == null || !(player.CurrentTarget is L2Player))
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.TARGET_IS_INCORRECT);
                player.sendActionFailed();
                return;
            }

            target = (L2Player)player.CurrentTarget;
            if (target.TradeState != 0)
            {
                player.sendPacket(new SystemMessage(SystemMessage.SystemMessageId.S1_ALREADY_TRADING).AddPlayerName(target.Name));
                player.sendActionFailed();
                return;
            }

            if (target.PartyState == 1)
            {
                player.sendPacket(new SystemMessage(SystemMessage.SystemMessageId.S1_IS_BUSY_TRY_LATER).AddPlayerName(target.Name));
                player.sendActionFailed();
                return;
            }

            if (!Calcs.checkIfInRange(150, player, target, true))
            {
                player.sendActionFailed();
                return;
            }

            player.sendPacket(new SystemMessage(SystemMessage.SystemMessageId.REQUEST_S1_FOR_TRADE).AddPlayerName(target.Name));
            target.requester = player;
            player.requester = target;
            target.sendPacket(new SendTradeRequest(player.ObjID));
            target.TradeState = 2; // жмакает ответ
            player.TradeState = 1; // запросил
        }
    }
}