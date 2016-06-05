namespace L2dotNET.GameService.network.serverpackets
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