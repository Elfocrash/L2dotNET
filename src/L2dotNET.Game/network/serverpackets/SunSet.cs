namespace L2dotNET.GameService.network.serverpackets
{
    class SunSet : GameServerNetworkPacket
    {
        protected internal override void write()
        {
            writeC(0x1d);
        }
    }
}