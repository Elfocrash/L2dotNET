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
            Makeme(client, data);
        }

        private int _clanId;

        public override void Read()
        {
            _clanId = ReadD();
        }

        public override void Run()
        {
            L2Player player = Client.CurrentPlayer;

            L2Clan clan = ClanTable.Instance.GetClan(_clanId);
            if (clan != null)
            {
                player.SendPacket(new PledgeInfo(clan.ClanId, clan.Name, clan.AllianceName));
            }
        }
    }
}