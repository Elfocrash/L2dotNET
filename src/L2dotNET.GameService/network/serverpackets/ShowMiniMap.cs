namespace L2dotNET.GameService.Network.Serverpackets
{
    class ShowMiniMap : GameserverPacket
    {
        protected internal override void Write()
        {
            WriteByte(0x9d);
            WriteInt(1665);
            WriteInt(0); //SevenSigns.getInstance().getCurrentPeriod());
        }
    }
}