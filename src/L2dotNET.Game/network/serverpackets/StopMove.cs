using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class StopMove : GameServerNetworkPacket
    {
        private readonly int _id;
        private readonly int _x;
        private readonly int _y;
        private readonly int _z;
        private readonly int _h;

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