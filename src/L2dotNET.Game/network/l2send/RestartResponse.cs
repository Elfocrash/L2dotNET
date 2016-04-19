
namespace L2dotNET.Game.network.l2send
{
    class RestartResponse : GameServerNetworkPacket
    {
        protected internal override void write()
        {
            writeC(0x71);
            writeD(0x01);
        }
    }
}
