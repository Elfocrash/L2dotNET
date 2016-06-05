namespace L2dotNET.GameService.Network.Serverpackets
{
    class ObservationReturn : GameServerNetworkPacket
    {
        private readonly int _x;
        private readonly int _y;
        private readonly int _z;

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