using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Clientpackets.PartyAPI
{
    class RequestWithDrawalParty : GameServerNetworkRequest
    {
        public RequestWithDrawalParty(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            // nothing
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            if (player.Party == null)
            {
                player.sendActionFailed();
                return;
            }

            player.Party.Leave(player);
        }
    }
}