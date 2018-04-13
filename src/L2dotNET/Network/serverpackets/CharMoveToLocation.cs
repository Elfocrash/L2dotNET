using L2dotNET.Models;
using L2dotNET.World;

namespace L2dotNET.Network.serverpackets
{
    class CharMoveToLocation : GameserverPacket
    {
        private readonly L2Object _obj;

        public CharMoveToLocation(L2Object obj)
        {
            _obj = obj;
        }

        public override void Write()
        {
            WriteByte(0x01);

            WriteInt(_obj.ObjId);

            WriteInt(_obj.DestX);
            WriteInt(_obj.DestY);
            WriteInt(_obj.DestZ);

            WriteInt(_obj.X);
            WriteInt(_obj.Y);
            WriteInt(_obj.Z);
        }
    }
}