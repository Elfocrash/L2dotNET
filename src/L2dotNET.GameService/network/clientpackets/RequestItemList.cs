using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestItemList : GameServerNetworkRequest
    {
        public RequestItemList(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        public override void Read()
        {
            // do nothing
        }

        public override void Run()
        {
            L2Player player = Client.CurrentPlayer;
            player.SendItemList(true);
        }
    }
}