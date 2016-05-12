using L2dotNET.GameService.model.communities;

namespace L2dotNET.GameService.network.l2send
{
    class PledgeReceiveMemberInfo : GameServerNetworkPacket
    {
        private ClanMember Member;
        public PledgeReceiveMemberInfo(ClanMember cm)
        {
            Member = cm;
        }

        protected internal override void write()
        {
            writeC(0xfe);
            writeH(0x3e);

            writeD(Member.ClanType);
            writeS(Member.Name);
            writeS(Member.NickName);
            writeD(Member.ClanPrivs);
            writeS(Member._pledgeTypeName);
            writeS(Member._ownerName); // name of this member's apprentice/sponsor
        }
    }
}
