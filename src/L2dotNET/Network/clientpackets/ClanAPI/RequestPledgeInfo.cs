using System;
using System.Threading.Tasks;

namespace L2dotNET.Network.clientpackets.ClanAPI
{
    class RequestPledgeInfo : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _clanId;

        public RequestPledgeInfo(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _clanId = packet.ReadInt();
        }

        public override async Task RunImpl()
        {
            await Task.FromResult(1);
            //L2Player player = _client.CurrentPlayer;

            //L2Clan clan = ClanTable.Instance.GetClan(_clanId);
            //if (clan != null)
            //    player.SendPacket(new PledgeInfo(clan.ClanId, clan.Name, clan.AllianceName));
        }
    }
}