namespace L2dotNET.Network.serverpackets
{
    class PledgeShowMemberListDeleteAll : GameserverPacket
    {
        public override void Write()
        {
            WriteByte(0x88);
        }
    }
}