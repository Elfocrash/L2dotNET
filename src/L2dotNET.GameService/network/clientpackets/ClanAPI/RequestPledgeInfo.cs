using L2dotNET.GameService.Config;
using L2dotNET.GameService.Model.Communities;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.Tables;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets.ClanAPI
{
    class RequestPledgeInfo : PacketBase
    {
        private int _clanId;
        private readonly GameClient _client;

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
            {
                player.SendPacket(new PledgeInfo(clan.ClanId, clan.Name, clan.AllianceName));
            }
        }
    }
}