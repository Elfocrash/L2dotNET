using L2dotNET.GameService.Config;
using L2dotNET.GameService.Model.Player;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets.PartyAPI
{
    class RequestPartyLootModification : PacketBase
    {
        private byte _mode;
        private readonly GameClient _client;

        public RequestPartyLootModification(Packet packet, GameClient client)
        {
            packet.MoveOffset(2);
            _client = client;
            _mode = packet.ReadByte();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            if (player.Party == null)
            {
                player.SendActionFailed();
                return;
            }

            if ((_mode < player.Party.ItemLooter) || (_mode > player.Party.ItemOrderSpoil) || (_mode == player.Party.ItemDistribution) || (player.Party.Leader.ObjId != player.ObjId))
            {
                player.SendActionFailed();
                return;
            }

            player.Party.VoteForLootChange(_mode);
        }
    }
}