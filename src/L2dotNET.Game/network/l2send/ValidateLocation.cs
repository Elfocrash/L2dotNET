
namespace L2dotNET.Game.network.l2send
{
    class ValidateLocation : GameServerNetworkPacket
    {
        private int _x, _id;
        private int _y;
        private int _z;
        private int _heading;

        public ValidateLocation(int _id, int _x, int _y, int _z, int _heading)
        {
            this._id = _id;
            this._x = _x;
            this._y = _y;
            this._z = _z;
            this._heading = _heading;
        }

        protected internal override void write()
        {
            writeC(0x79);

            writeD(_id);
            writeD(_x);
            writeD(_y);
            writeD(_z);
            writeD(_heading);
        }
    }
}
