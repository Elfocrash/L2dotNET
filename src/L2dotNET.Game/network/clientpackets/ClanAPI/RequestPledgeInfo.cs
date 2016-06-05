using L2dotNET.GameService.Model.communities;
using L2dotNET.GameService.Model.player;
using L2dotNET.GameService.network.serverpackets;
using L2dotNET.GameService.tables;

namespace L2dotNET.GameService.network.clientpackets.ClanAPI
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

            L2Clan clan = ClanTable.Instance.GetClan(_clanId);
            if (clan != null)
                player.sendPacket(new PledgeInfo(clan.ClanID, clan.Name, clan.AllianceName));
        }
    }
}