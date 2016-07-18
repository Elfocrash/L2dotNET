namespace L2dotNET.GameService.Network.Serverpackets
{
    class PledgeShowMemberListDeleteAll : GameserverPacket
    {
        protected internal override void Write()
        {
            WriteByte(0x88);
        }
    }
}