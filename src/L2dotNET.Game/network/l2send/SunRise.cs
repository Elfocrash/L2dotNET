
namespace L2dotNET.Game.network.l2send
{
    class SunRise : GameServerNetworkPacket
    {
        protected internal override void write()
        {
            writeC(0x12);
        }
    }
}
