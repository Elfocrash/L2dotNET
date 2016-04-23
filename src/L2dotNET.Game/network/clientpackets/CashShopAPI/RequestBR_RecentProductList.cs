using L2dotNET.Game.tables;

namespace L2dotNET.Game.network.l2recv
{
    class RequestBR_RecentProductList : GameServerNetworkRequest
    {
        public RequestBR_RecentProductList(GameClient client, byte[] data)
        {
            base.makeme(client, data, 2);
        }

        public override void read()
        {
            // nothing
        }

        public override void run()
        {
           // CashShop.getInstance().showRecentList(Client.CurrentPlayer);
        }
    }
}
