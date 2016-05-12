
namespace L2dotNET.GameService.network.l2send
{
    class ExShowOwnthingPos : GameServerNetworkPacket
    {
        protected internal override void write()
        {
            writeC(0xfe);
            writeH(0x93);

            writeD(0);
            writeD(0);
        }
    }
}
