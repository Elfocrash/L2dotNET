using L2dotNET.GameService.model.communities;
using L2dotNET.GameService.network.l2send;
using L2dotNET.GameService.tables;

namespace L2dotNET.GameService.network.l2recv
{
    class RequestPledgeInfo : GameServerNetworkRequest
    {
        public RequestPledgeInfo(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        private int _clanId;
        public override void read()
        {
            _clanId = readD();
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            L2Clan clan = ClanTable.getInstance().getClan(_clanId);
            if (clan != null)
                player.sendPacket(new PledgeInfo(clan.ClanID, clan.Name, clan.AllianceName));
        }
    }
}
