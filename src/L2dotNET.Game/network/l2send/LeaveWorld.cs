
namespace L2dotNET.Game.network.l2send
{
    class LeaveWorld : GameServerNetworkPacket
    {
        protected internal override void write()
        {
            writeC(0x7e);
        }
    }
}
