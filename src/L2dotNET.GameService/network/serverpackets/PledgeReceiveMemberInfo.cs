using L2dotNET.GameService.Model.Communities;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class PledgeReceiveMemberInfo : GameserverPacket
    {
        private readonly ClanMember _member;

        public PledgeReceiveMemberInfo(ClanMember cm)
        {
            _member = cm;
        }

        public override void Write()
        {
            WriteByte(0xfe);
            WriteShort(0x3e);

            WriteInt(_member.ClanType);
            WriteString(_member.Name);
            WriteString(_member.NickName);
            WriteInt(_member.ClanPrivs);
            WriteString(_member.PledgeTypeName);
            WriteString(_member.OwnerName); // name of this member's apprentice/sponsor
        }
    }
}