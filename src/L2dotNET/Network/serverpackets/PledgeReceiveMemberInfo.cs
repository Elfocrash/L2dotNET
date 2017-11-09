namespace L2dotNET.Network.serverpackets
{
    class PledgeReceiveMemberInfo : GameserverPacket
    {
        public PledgeReceiveMemberInfo()
        {
        }

        public override void Write()
        {
            WriteByte(0xfe);
            WriteShort(0x3e);
        }
    }
}