using L2dotNET.GameService.model.communities;
using L2dotNET.GameService.network.l2send;

namespace L2dotNET.GameService.network.l2recv
{
    class RequestPledgeMemberInfo : GameServerNetworkRequest
    {
        public RequestPledgeMemberInfo(GameClient client, byte[] data)
        {
            base.makeme(client, data, 2);
        }

        private int _unk1;
        private string _player;
        public override void read()
        {
            _unk1 = readD();
            _player = readS();
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            if (player.Clan == null)
            {
                player.sendActionFailed();
                return;
            }


            L2Clan clan = player.Clan;

            ClanMember m = null;
            foreach (ClanMember cm in clan.getClanMembers())
            {
                if (cm.Name.Equals(_player))
                {
                    m = cm;
                    break;
                }
            }

            if (m == null)
            {
                player.sendActionFailed();
                return;
            }

            player.sendPacket(new PledgeReceiveMemberInfo(m));
        }
    }
}
