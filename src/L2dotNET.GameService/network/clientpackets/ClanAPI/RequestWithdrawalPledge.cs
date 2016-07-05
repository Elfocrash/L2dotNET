using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Clientpackets.ClanAPI
{
    class RequestWithdrawalPledge : GameServerNetworkRequest
    {
        public RequestWithdrawalPledge(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        public override void Read()
        {
            // not actions
        }

        public override void Run()
        {
            L2Player player = Client.CurrentPlayer;

            if (player.Clan != null)
            {
                player.Clan.Leave(player);
            }
            else
            {
                player.SendActionFailed();
            }
        }
    }
}