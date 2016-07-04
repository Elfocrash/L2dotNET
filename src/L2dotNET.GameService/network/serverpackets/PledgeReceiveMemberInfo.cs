using L2dotNET.GameService.Model.Communities;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class PledgeReceiveMemberInfo : GameServerNetworkPacket
    {
        private readonly ClanMember _member;

        public PledgeReceiveMemberInfo(ClanMember cm)
        {
            _member = cm;
        }

        protected internal override void Write()
        {
            WriteC(0xfe);
            WriteH(0x3e);

            WriteD(_member.ClanType);
            WriteS(_member.Name);
            WriteS(_member.NickName);
            WriteD(_member.ClanPrivs);
            WriteS(_member.PledgeTypeName);
            WriteS(_member.OwnerName); // name of this member's apprentice/sponsor
        }
    }
}