namespace L2dotNET.GameService.network.serverpackets
{
    class SunRise : GameServerNetworkPacket
    {
        protected internal override void write()
        {
            writeC(0x1c);
        }
    }
}