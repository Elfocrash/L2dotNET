using L2dotNET.GameService.Model.player;

namespace L2dotNET.GameService.network.clientpackets.ClanAPI
{
    class RequestWithdrawalPledge : GameServerNetworkRequest
    {
        public RequestWithdrawalPledge(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        public override void read()
        {
            // not actions
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            if (player.Clan != null)
                player.Clan.Leave(player);
            else
                player.sendActionFailed();
        }
    }
}