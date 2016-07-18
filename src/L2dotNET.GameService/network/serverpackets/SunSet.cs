namespace L2dotNET.GameService.Network.Serverpackets
{
    class SunSet : GameserverPacket
    {
        protected internal override void Write()
        {
            WriteByte(0x1d);
        }
    }
}