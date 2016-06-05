using L2dotNET.GameService.Model.player;

namespace L2dotNET.GameService.network.clientpackets
{
    class RequestItemList : GameServerNetworkRequest
    {
        public RequestItemList(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        public override void read()
        {
            // do nothing
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;
            player.sendItemList(true);
        }
    }
}