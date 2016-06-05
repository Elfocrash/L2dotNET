using L2dotNET.GameService.world;

namespace L2dotNET.GameService.network.l2send
{
    class TargetUnselected : GameServerNetworkPacket
    {
        private readonly int _id;
        private readonly int _x;
        private readonly int _y;
        private readonly int _z;
        public TargetUnselected(L2Object obj)
        {
            _id = obj.ObjID;
            _x = obj.X;
            _y = obj.Y;
            _z = obj.Z;
        }

        protected internal override void write()
        {
            writeC(0x2a);
            writeD(_id);
            writeD(_x);
            writeD(_y);
            writeD(_z);
        }
    }
}
