using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestItemList : GameServerNetworkRequest
    {
        public RequestItemList(GameClient client, byte[] data)
        {
            makeme(client, data);
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