using L2dotNET.Game.tables;

namespace L2dotNET.Game.network.l2recv
{
    class RequestBR_ProductList : GameServerNetworkRequest
    {
        public RequestBR_ProductList(GameClient client, byte[] data)
        {
            base.makeme(client, data, 2);
        }

        public override void read()
        {
            // nothing
        }

        public override void run()
        {
            CashShop.getInstance().showList(Client.CurrentPlayer);
        }
    }
}
