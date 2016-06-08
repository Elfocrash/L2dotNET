using L2dotNET.GameService.Model.Communities;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.Tables;

namespace L2dotNET.GameService.Network.Clientpackets.ClanAPI
{
    class RequestPledgeInfo : GameServerNetworkRequest
    {
        public RequestPledgeInfo(GameClient client, byte[] data)
        {
            makeme(client, data);
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