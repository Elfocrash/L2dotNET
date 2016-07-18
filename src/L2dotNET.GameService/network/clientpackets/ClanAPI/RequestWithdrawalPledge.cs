using L2dotNET.GameService.Model.Player;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets.ClanAPI
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