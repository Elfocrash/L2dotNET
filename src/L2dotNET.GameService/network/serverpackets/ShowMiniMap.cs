namespace L2dotNET.GameService.Network.Serverpackets
{
    class ShowMiniMap : GameServerNetworkPacket
    {
        protected internal override void write()
        {
            writeC(0x9d);
            writeD(1665);
            writeD(0); //SevenSigns.getInstance().getCurrentPeriod());
        }
    }
}