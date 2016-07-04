using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class MoveToPawn : GameServerNetworkPacket
    {
        private readonly int _id;
        private readonly int _target;
        private readonly int _dist;
        private int _x;
        private readonly int _tx;
        private int _y;
        private readonly int _ty;
        private int _z;
        private readonly int _tz;

        public MoveToPawn(int id, L2Object target, int dist, int x, int y, int z)
        {
            _id = id;
            _target = target.ObjId;
            _dist = dist;
            _x = x;
            _y = y;
            _z = z;
            _tx = target.X;
            _ty = target.Y;
            _tz = target.Z;
        }

        protected internal override void Write()
        {
            WriteC(0x60);

            WriteD(_id);
            WriteD(_target);
            WriteD(_dist);

            //writeD(_x);
            //writeD(_y);
            //writeD(_z);
            WriteD(_tx);
            WriteD(_ty);
            WriteD(_tz);
        }
    }
}