using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class TargetUnselected : GameServerNetworkPacket
    {
        private readonly int _id;
        private readonly int _x;
        private readonly int _y;
        private readonly int _z;

        public TargetUnselected(L2Object obj)
        {
            _id = obj.ObjId;
            _x = obj.X;
            _y = obj.Y;
            _z = obj.Z;
        }

        protected internal override void Write()
        {
            WriteC(0x2a);
            WriteD(_id);
            WriteD(_x);
            WriteD(_y);
            WriteD(_z);
        }
    }
}