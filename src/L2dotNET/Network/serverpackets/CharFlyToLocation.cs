using L2dotNET.Models;

namespace L2dotNET.Network.serverpackets
{
    class CharFlyToLocation : GameserverPacket
    {
        private readonly L2Object _obj;
        private readonly int _id;

        public CharFlyToLocation(L2Object obj, FlyType type)
        {
            _obj = obj;
            _id = (int)type;
        }

        public override void Write()
        {
            WriteByte(0xC5);

            WriteInt(_obj.ObjectId);

            WriteInt(_obj.DestX);
            WriteInt(_obj.DestY);
            WriteInt(_obj.DestZ);

            WriteInt(_obj.X);
            WriteInt(_obj.Y);
            WriteInt(_obj.Z);

            WriteInt(_id);
        }
    }
}