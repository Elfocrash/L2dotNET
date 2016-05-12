using L2dotNET.GameService.network.l2send;
using L2dotNET.GameService.tools;

namespace L2dotNET.GameService.network.l2recv
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
                player.sendSystemMessage(142);//You are already trading with someone.
                player.sendActionFailed();
                return;
            }

            if (player.ObjID == targetId)
            {
                player.sendSystemMessage(51);//You cannot use this on yourself.
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
                player.sendSystemMessage(144);//That is an incorrect target.
                player.sendActionFailed();
                return;
            }

            target = (L2Player)player.CurrentTarget;
            if (target.TradeState != 0)
            {
                player.sendPacket(new SystemMessage(143).AddPlayerName(target.Name));//$c1 is already trading with another person. Please try again later.
                player.sendActionFailed();
                return;
            }

            if (target.PartyState == 1)
            {
                player.sendPacket(new SystemMessage(153).AddPlayerName(target.Name));//$c1 is on another task. Please try again later.
                player.sendActionFailed();
                return;
            }

            if (!Calcs.checkIfInRange(150, player, target, true))
            {
                player.sendActionFailed();
                return;
            }

            player.sendPacket(new SystemMessage(118).AddPlayerName(target.Name));//You have requested a trade with $c1.
            target.requester = player;
            player.requester = target;
            target.sendPacket(new SendTradeRequest(player.ObjID));
            target.TradeState = 2; // жмакает ответ
            player.TradeState = 1; // запросил
        }
    }
}
