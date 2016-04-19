using L2dotNET.Game.world;

namespace L2dotNET.Game.network.l2send
{
    class TargetSelected : GameServerNetworkPacket
    {
        private int _x;
        private int _y;
        private int _z;
        private int _objectId;
        private int _targetObjId;
        public TargetSelected(int selecterId, L2Object target)
        {
            _objectId = selecterId;
            _targetObjId = target.ObjID;
            _x = target.X;
            _y = target.Y;
            _z = target.Z;
        }

        protected internal override void write()
        {
            writeC(0x23);
            writeD(_objectId);
            writeD(_targetObjId);
            writeD(_x);
            writeD(_y);
            writeD(_z);
            writeD(0x00);
        }
    }
}
