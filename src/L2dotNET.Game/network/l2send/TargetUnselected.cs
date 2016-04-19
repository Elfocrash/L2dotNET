using L2dotNET.Game.world;

namespace L2dotNET.Game.network.l2send
{
    class TargetUnselected : GameServerNetworkPacket
    {
        private int _id;
        private int _x;
        private int _y;
        private int _z;
        public TargetUnselected(L2Object obj)
        {
            _id = obj.ObjID;
            _x = obj.X;
            _y = obj.Y;
            _z = obj.Z;
        }

        protected internal override void write()
        {
            writeC(0x24);
            writeD(_id);
            writeD(_x);
            writeD(_y);
            writeD(_z);
            writeD(0x00); //??
        }
    }
}
