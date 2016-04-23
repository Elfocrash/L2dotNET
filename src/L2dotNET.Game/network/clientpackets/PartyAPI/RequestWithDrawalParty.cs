
namespace L2dotNET.Game.network.l2recv
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
