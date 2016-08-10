using L2dotNET.model.communities;
using L2dotNET.model.player;
using L2dotNET.Network.serverpackets;
using L2dotNET.tables;

namespace L2dotNET.Network.clientpackets.ClanAPI
{
    class RequestPledgeInfo : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _clanId;

        public RequestPledgeInfo(Packet packet, GameClient client)
        {
            _client = client;
            _clanId = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            L2Clan clan = ClanTable.Instance.GetClan(_clanId);
            if (clan != null)
                player.SendPacket(new PledgeInfo(clan.ClanId, clan.Name, clan.AllianceName));
        }
    }
}