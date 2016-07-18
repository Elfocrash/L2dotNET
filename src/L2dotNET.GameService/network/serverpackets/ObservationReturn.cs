using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ObservationReturn : GameserverPacket
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

        public override void Write()
        {
            WriteByte(0xe0);
            WriteInt(_x);
            WriteInt(_y);
            WriteInt(_z);
        }
    }
}