namespace L2dotNET.Network.serverpackets
{
    class ShowMiniMap : GameserverPacket
    {
        public override void Write()
        {
            WriteByte(0x9d);
            WriteInt(1665);
            WriteInt(0); //SevenSigns.getInstance().getCurrentPeriod());
        }
    }
}