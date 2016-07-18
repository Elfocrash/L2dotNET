using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class TeleportToLocation : GameserverPacket
    {
        private readonly int _x;
        private readonly int _y;
        private readonly int _z;
        private readonly int _id;
        private int _heading;

        public TeleportToLocation(int id, int x, int y, int z, int h)
        {
            _x = x;
            _y = y;
            _z = z;
            _id = id;
            _heading = h;
        }

        public override void Write()
        {
            WriteByte(0x28);
            WriteInt(_id);
            WriteInt(_x);
            WriteInt(_y);
            WriteInt(_z);
        }
    }
}