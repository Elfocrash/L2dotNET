namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExShowOwnthingPos : GameServerNetworkPacket
    {
        protected internal override void Write()
        {
            WriteC(0xfe);
            WriteH(0x93);

            WriteD(0);
            WriteD(0);
        }
    }
}