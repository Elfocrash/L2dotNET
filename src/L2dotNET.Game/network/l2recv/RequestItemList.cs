
namespace L2dotNET.Game.network.l2recv
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
