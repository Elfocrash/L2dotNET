using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class TargetSelected : GameServerNetworkPacket
    {
        private readonly int _x;
        private readonly int _y;
        private readonly int _z;
        private readonly int _objectId;
        private readonly int _targetObjId;

        public TargetSelected(int selecterId, L2Object target)
        {
            _objectId = selecterId;
            _targetObjId = target.ObjId;
            _x = target.X;
            _y = target.Y;
            _z = target.Z;
        }

        protected internal override void write()
        {
            writeC(0x29);
            writeD(_objectId);
            writeD(_targetObjId);
            writeD(_x);
            writeD(_y);
            writeD(_z);
        }
    }
}