namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExBuySellListClose : GameServerNetworkPacket
    {
        protected internal override void Write()
        {
            WriteC(0xFE);
            WriteH(0xB7);
            WriteD(1);
            WriteH(0);
            WriteH(0);
            WriteC(1);
        }
    }
}