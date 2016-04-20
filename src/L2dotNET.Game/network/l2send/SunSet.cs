
namespace L2dotNET.Game.network.l2send
{
    class SunSet : GameServerNetworkPacket
    {
        protected internal override void write()
        {
            writeC(0x1d);
        }
    }
}
