namespace L2dotNET.Network.serverpackets
{
    class SunSet : GameserverPacket
    {
        public override void Write()
        {
            WriteByte(0x1d);
        }
    }
}