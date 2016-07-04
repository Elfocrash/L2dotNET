namespace L2dotNET.GameService.Network.Serverpackets
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

        protected internal override void Write()
        {
            WriteC(0xdf);
            WriteD(_x);
            WriteD(_y);
            WriteD(_z);
            WriteC(0x00);
            WriteC(0xc0);
            WriteC(0x00);
        }
    }
}