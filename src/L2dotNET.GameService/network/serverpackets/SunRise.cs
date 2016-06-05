namespace L2dotNET.GameService.Network.Serverpackets
{
    class SunRise : GameServerNetworkPacket
    {
        protected internal override void write()
        {
            writeC(0x1c);
        }
    }
}