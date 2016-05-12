using L2dotNET.GameService.world;

namespace L2dotNET.GameService.network.l2send
{
    class StopMove : GameServerNetworkPacket
    {
        private int _id;
        private int _x;
        private int _y;
        private int _z;
        private int _h;
        public StopMove(L2Character cha)
        {
            _id = cha.ObjID;
            _x = cha.X;
            _y = cha.Y;
            _z = cha.Z;
            _h = cha.Heading;
        }

        protected internal override void write()
        {
            writeC(0x47);
            writeD(_id);
            writeD(_x);
            writeD(_y);
            writeD(_z);
            writeD(_h);
        }
    }
}
