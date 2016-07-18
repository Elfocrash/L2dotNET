using System.Linq;
using L2dotNET.GameService.Config;
using L2dotNET.GameService.Model.Communities;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets.ClanAPI
{
    class RequestPledgeMemberInfo : PacketBase
    {
        private readonly GameClient _client;

        private int _unk1;
        private string _player;

        public RequestPledgeMemberInfo(Packet packet, GameClient client)
        {
            packet.MoveOffset(2);
            _client = client;
            _unk1 = packet.ReadInt();
            _player = packet.ReadString();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

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