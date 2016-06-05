namespace L2dotNET.GameService.Network.Serverpackets
{
    class Calculator : GameServerNetworkPacket
    {
        protected internal override void write()
        {
            writeC(0xe2);
            writeD(4393);
        }
    }
}