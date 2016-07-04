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

        public ValidateLocation(int _id, int x, int y, int z, int heading)
        {
            this._id = _id;
            this._x = x;
            this._y = y;
            this._z = z;
            this._heading = heading;
        }

        public ValidateLocation(L2Character character)
        {
            _id = character.ObjId;
            _x = character.X;
            _y = character.Y;
            _z = character.Z;
            _heading = character.Heading;
        }

        protected internal override void write()
        {
            writeC(0x61);

            writeD(_id);
            writeD(_x);
            writeD(_y);
            writeD(_z);
            writeD(_heading);
        }
    }
}