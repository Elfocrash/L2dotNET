namespace L2dotNET.GameService.Network.Serverpackets
{
    class SunRise : GameserverPacket
    {
        protected internal override void Write()
        {
            WriteByte(0x1c);
        }
    }
}