namespace L2dotNET.Network.serverpackets
{
    class SunRise : GameserverPacket
    {
        public override void Write()
        {
            WriteByte(0x1c);
        }
    }
}