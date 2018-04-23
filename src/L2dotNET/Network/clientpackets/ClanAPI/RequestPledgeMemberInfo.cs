using System;

namespace L2dotNET.Network.clientpackets.ClanAPI
{
    class RequestPledgeMemberInfo : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _unk1;
        private readonly string _player;

        public RequestPledgeMemberInfo(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            packet.MoveOffset(2);
            _client = client;
            _unk1 = packet.ReadInt();
            _player = packet.ReadString();
        }

        public override void RunImpl()
        {
            //L2Player player = _client.CurrentPlayer;

            //if (player.Clan == null)
            //{
            //    player.SendActionFailed();
            //    return;
            //}

            //L2Clan clan = player.Clan;

            //ClanMember m = clan.GetClanMembers().FirstOrDefault(cm => cm.Name.Equals(_player));

            //if (m == null)
            //{
            //    player.SendActionFailed();
            //    return;
            //}

            //player.SendPacket(new PledgeReceiveMemberInfo(m));
        }
    }
}