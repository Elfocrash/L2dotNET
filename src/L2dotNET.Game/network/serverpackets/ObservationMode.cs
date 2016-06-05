namespace L2dotNET.GameService.network.l2send
{
    class ObservationMode : GameServerNetworkPacket
    {
        private readonly int _x;
        private readonly int _y;
        private readonly int _z;

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