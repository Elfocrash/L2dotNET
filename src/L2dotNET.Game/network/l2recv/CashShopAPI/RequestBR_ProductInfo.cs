using L2dotNET.Game.tables;

namespace L2dotNET.Game.network.l2recv
{
    class RequestBR_ProductInfo : GameServerNetworkRequest
    {
        private int iProductID;
        public RequestBR_ProductInfo(GameClient client, byte[] data)
        {
            base.makeme(client, data, 2);
        }

        public override void read()
        {
            iProductID = readD();
        }

        public override void run()
        {
            CashShop.getInstance().showItemInfo(Client.CurrentPlayer, iProductID);
        }
    }
}
