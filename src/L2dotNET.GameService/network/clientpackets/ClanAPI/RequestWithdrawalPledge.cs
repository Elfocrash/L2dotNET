using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Clientpackets.ClanAPI
{
    class RequestWithdrawalPledge : GameServerNetworkRequest
    {
        public RequestWithdrawalPledge(GameClient client, byte[] data)
        {
            makeme(client, data);
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