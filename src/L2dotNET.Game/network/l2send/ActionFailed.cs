
namespace L2dotNET.Game.network.l2send
{
    class ActionFailed : GameServerNetworkPacket
    {
        protected internal override void write()
        {
            writeC(0x25);
        }
    }
}
