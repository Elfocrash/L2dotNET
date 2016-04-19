using L2dotNET.Game.tables;

namespace L2dotNET.Game.network.l2recv
{
    class RequestBR_BuyProduct : GameServerNetworkRequest
    {
        private int clientID;
        private int count;
        public RequestBR_BuyProduct(GameClient client, byte[] data)
        {
            base.makeme(client, data, 2);
        }

        public override void read()
        {
            clientID = readD();
            count = readD();
        }

        public override void run()
        {
            CashShop.getInstance().requestBuyItem(Client.CurrentPlayer, clientID, count);
        }
    }
}
