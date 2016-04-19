
namespace L2dotNET.Game.network.l2send
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
