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

        protected internal override void Write()
        {
            WriteC(0xe0);
            WriteD(_x);
            WriteD(_y);
            WriteD(_z);
        }
    }
}