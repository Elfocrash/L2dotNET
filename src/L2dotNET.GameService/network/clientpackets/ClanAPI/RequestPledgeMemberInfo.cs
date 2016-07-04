using System.Linq;
using L2dotNET.GameService.Model.Communities;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets.ClanAPI
{
    class RequestPledgeMemberInfo : GameServerNetworkRequest
    {
        public RequestPledgeMemberInfo(GameClient client, byte[] data)
        {
            Makeme(client, data, 2);
        }

        private int _unk1;
        private string _player;

        public override void Read()
        {
            _unk1 = ReadD();
            _player = ReadS();
        }

        public override void Run()
        {
            L2Player player = Client.CurrentPlayer;

            if (player.Clan == null)
            {
                player.SendActionFailed();
                return;
            }

            L2Clan clan = player.Clan;

            ClanMember m = clan.GetClanMembers().FirstOrDefault(cm => cm.Name.Equals(_player));

            if (m == null)
            {
                player.SendActionFailed();
                return;
            }

            player.SendPacket(new PledgeReceiveMemberInfo(m));
        }
    }
}