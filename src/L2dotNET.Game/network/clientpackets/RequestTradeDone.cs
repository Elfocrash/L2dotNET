using L2dotNET.Game.managers;
using L2dotNET.Game.network.l2send;

namespace L2dotNET.Game.network.l2recv
{
    class RequestTradeDone : GameServerNetworkRequest
    {
        private bool bDone;
        public RequestTradeDone(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        public override void read()
        {
            bDone = readD() == 1;
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            if (player.TradeState < 3)// умник
            {
                player.sendActionFailed();
                return;
            }

            if (player.requester == null)
            {
                player.sendMessage("Your trade requestor has logged off.");
                player.sendActionFailed();
                player.TradeState = 0;

                if (player.currentTrade != null)
                    player.currentTrade.Clear();

                return;
            }

            if (bDone)
            {
                player.TradeState = 4;
                player.requester.sendPacket(new SystemMessage(121).AddPlayerName(player.Name));//$c1 has confirmed the trade.

                if (player.requester.TradeState == 4)
                    TradeManager.getInstance().PersonalTrade(player, player.requester);
            }
            else
            {
                TradeDone end = new TradeDone(false);
                player.TradeState = 0;
                player.currentTrade.Clear();
                player.sendPacket(end);
                player.requester.sendPacket(new SystemMessage(124).AddPlayerName(player.Name));//$c1 has cancelled the trade.

                player.requester.TradeState = 0;
                player.requester.currentTrade.Clear();
                player.requester.sendPacket(end);
                player.requester = null;
            }
        }
    }
}
