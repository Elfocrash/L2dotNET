using L2dotNET.Models;

namespace L2dotNET.Network.serverpackets
{
    class ValidateLocation : GameserverPacket
    {
        private readonly int _x;
        private readonly int _id;
        private readonly int _y;
        private readonly int _z;
        private readonly int _heading;

        public ValidateLocation(int id, int x, int y, int z, int heading)
        {
            _id = id;
            _x = x;
            _y = y;
            _z = z;
            _heading = heading;
        }

        public ValidateLocation(L2Character character)
        {
            _id = character.ObjectId;
            _x = character.X;
            _y = character.Y;
            _z = character.Z;
            _heading = character.Heading;
        }

        public override void Write()
        {
            WriteByte(0x61);

            WriteInt(_id);
            WriteInt(_x);
            WriteInt(_y);
            WriteInt(_z);
            WriteInt(_heading);
        }
    }
}