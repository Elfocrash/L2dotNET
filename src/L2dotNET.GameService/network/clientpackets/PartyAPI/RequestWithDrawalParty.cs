using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Clientpackets.PartyAPI
{
    class RequestWithDrawalParty : GameServerNetworkRequest
    {
        public RequestWithDrawalParty(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        public override void Read()
        {
            // nothing
        }

        public override void Run()
        {
            L2Player player = Client.CurrentPlayer;

            if (player.Party == null)
            {
                player.SendActionFailed();
                return;
            }

            player.Party.Leave(player);
        }
    }
}