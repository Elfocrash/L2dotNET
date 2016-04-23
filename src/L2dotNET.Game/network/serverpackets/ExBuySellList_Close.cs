
namespace L2dotNET.Game.network.l2send
{
    class ExBuySellList_Close : GameServerNetworkPacket
    {
        protected internal override void write()
        {
            writeC(0xFE);
            writeH(0xB7);
            writeD(1);
            writeH(0);
            writeH(0);
            writeC(1);
        }
    }
}
