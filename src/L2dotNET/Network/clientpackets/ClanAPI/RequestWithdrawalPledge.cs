using L2dotNET.model.player;

namespace L2dotNET.Network.clientpackets.ClanAPI
{
    class RequestWithdrawalPledge : PacketBase
    {
        private readonly GameClient _client;

        public RequestWithdrawalPledge(Packet packet, GameClient client)
        {
            _client = client;
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            if (player.Clan != null)
                player.Clan.Leave(player);
            else
                player.SendActionFailed();
        }
    }
}