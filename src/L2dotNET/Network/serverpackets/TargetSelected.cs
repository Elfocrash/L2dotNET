using L2dotNET.Models;

namespace L2dotNET.Network.serverpackets
{
    class TargetSelected : GameserverPacket
    {
        private readonly int _x;
        private readonly int _y;
        private readonly int _z;
        private readonly int _objectId;
        private readonly int _targetObjId;

        public TargetSelected(int selecterId, L2Object target)
        {
            _objectId = selecterId;
            _targetObjId = target.CharacterId;
            _x = target.X;
            _y = target.Y;
            _z = target.Z;
        }

        public override void Write()
        {
            WriteByte(0x29);
            WriteInt(_objectId);
            WriteInt(_targetObjId);
            WriteInt(_x);
            WriteInt(_y);
            WriteInt(_z);
        }
    }
}