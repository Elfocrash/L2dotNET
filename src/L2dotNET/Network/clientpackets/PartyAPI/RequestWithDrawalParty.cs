using L2dotNET.Models.player;

namespace L2dotNET.Network.clientpackets.PartyAPI
{
    class RequestWithDrawalParty : PacketBase
    {
        private readonly GameClient _client;

        public RequestWithDrawalParty(Packet packet, GameClient client)
        {
            _client = client;
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            if (player.Party == null)
            {
                player.SendActionFailed();
                return;
            }

            player.Party.Leave(player);
        }
    }
}