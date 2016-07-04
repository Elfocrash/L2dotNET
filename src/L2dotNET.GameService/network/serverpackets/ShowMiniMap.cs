namespace L2dotNET.GameService.Network.Serverpackets
{
    class ShowMiniMap : GameServerNetworkPacket
    {
        protected internal override void Write()
        {
            WriteC(0x9d);
            WriteD(1665);
            WriteD(0); //SevenSigns.getInstance().getCurrentPeriod());
        }
    }
}