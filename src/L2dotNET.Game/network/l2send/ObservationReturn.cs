
namespace L2dotNET.Game.network.l2send
{
    class ObservationReturn : GameServerNetworkPacket
    {
        private int _x;
        private int _y;
        private int _z;
        public ObservationReturn(int x, int y, int z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        protected internal override void write()
        {
            writeC(0xe0);
            writeD(_x);
            writeD(_y);
            writeD(_z);
        }
    }
}
