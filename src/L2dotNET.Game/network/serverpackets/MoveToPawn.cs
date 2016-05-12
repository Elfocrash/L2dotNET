using L2dotNET.GameService.world;

namespace L2dotNET.GameService.network.l2send
{
    class MoveToPawn : GameServerNetworkPacket
    {
        private int _id;
        private int _target;
        private int _dist;
        private int _x, _tx;
        private int _y, _ty;
        private int _z, _tz;

        public MoveToPawn(int id, L2Object target, int dist, int x, int y, int z)
        {
            _id = id;
            _target = target.ObjID;
            _dist = dist;
            _x = x;
            _y = y;
            _z = z;
            _tx = target.X;
            _ty = target.Y;
            _tz = target.Z;
        }

        protected internal override void write()
        {
            writeC(0x60);

            writeD(_id);
            writeD(_target);
            writeD(_dist);

            //writeD(_x);
            //writeD(_y);
            //writeD(_z);
            writeD(_tx);
            writeD(_ty);
            writeD(_tz);
        }
    }
}
