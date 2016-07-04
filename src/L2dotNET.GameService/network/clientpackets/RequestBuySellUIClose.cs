using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestBuySellUiClose : GameServerNetworkRequest
    {
        public RequestBuySellUiClose(GameClient client, byte[] data)
        {
            Makeme(client, data, 2);
        }

        public override void Read()
        {
            // nothing
        }

        public override void Run()
        {
            L2Player player = Client.CurrentPlayer;

            player.SendItemList(true);
        }
    }
}