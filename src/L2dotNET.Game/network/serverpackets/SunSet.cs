namespace L2dotNET.GameService.Network.Serverpackets
{
    class SunSet : GameServerNetworkPacket
    {
        protected internal override void write()
        {
            writeC(0x1d);
        }
    }
}