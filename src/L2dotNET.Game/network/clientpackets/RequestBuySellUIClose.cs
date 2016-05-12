
namespace L2dotNET.GameService.network.l2recv
{
    class RequestBuySellUIClose : GameServerNetworkRequest
    {
        public RequestBuySellUIClose(GameClient client, byte[] data)
        {
            base.makeme(client, data, 2);
        }

        public override void read()
        {
            // nothing
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            player.sendItemList(true);
        }
    }
}
