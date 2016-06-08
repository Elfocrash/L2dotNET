using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestBuySellUIClose : GameServerNetworkRequest
    {
        public RequestBuySellUIClose(GameClient client, byte[] data)
        {
            makeme(client, data, 2);
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