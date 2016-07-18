namespace L2dotNET.GameService.Network.Serverpackets
{
    class LeaveWorld : GameserverPacket
    {
        protected internal override void Write()
        {
            WriteByte(0x7e);
        }
    }
}