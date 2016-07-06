using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ValidateLocation : GameServerNetworkPacket
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
            _id = character.ObjId;
            _x = character.X;
            _y = character.Y;
            _z = character.Z;
            _heading = character.Heading;
        }

        protected internal override void Write()
        {
            WriteC(0x61);

            WriteD(_id);
            WriteD(_x);
            WriteD(_y);
            WriteD(_z);
            WriteD(_heading);
        }
    }
}