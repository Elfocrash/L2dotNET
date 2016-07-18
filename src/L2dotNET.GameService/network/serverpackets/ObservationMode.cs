using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ObservationMode : GameserverPacket
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

        public override void Write()
        {
            WriteByte(0xdf);
            WriteInt(_x);
            WriteInt(_y);
            WriteInt(_z);
            WriteByte(0x00);
            WriteByte(0xc0);
            WriteByte(0x00);
        }
    }
}