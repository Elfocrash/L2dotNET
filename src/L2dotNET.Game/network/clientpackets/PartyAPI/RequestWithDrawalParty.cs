using L2dotNET.GameService.Model.player;

namespace L2dotNET.GameService.network.clientpackets.PartyAPI
{
    class RequestWithDrawalParty : GameServerNetworkRequest
    {
        public RequestWithDrawalParty(GameClient client, byte[] data)
        {
            base.makeme(client, data);
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