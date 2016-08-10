namespace L2dotNET.Network.serverpackets
{
    class LeaveWorld : GameserverPacket
    {
        public override void Write()
        {
            WriteByte(0x7e);
        }
    }
}