
namespace L2dotNET.Game.network.l2send
{
    class ObservationMode : GameServerNetworkPacket
    {
        private int _x;
        private int _y;
        private int _z;
        public ObservationMode(int x, int y, int z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        protected internal override void write()
        {
            writeC(0xdf);
            writeD(_x);
            writeD(_y);
            writeD(_z);
            writeC(0x00);
            writeC(0xc0);
            writeC(0x00);
        }
    }
}
