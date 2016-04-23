
namespace L2dotNET.Game.network.l2send
{
    class TeleportToLocation : GameServerNetworkPacket
    {
        private int _x;
        private int _y;
        private int _z;
        private int _id;
        private int _heading;
        public TeleportToLocation(int id, int x, int y, int z, int h)
        {
            _x = x;
            _y = y; 
            _z = z;
            _id = id;
            _heading = h;
        }

        protected internal override void write()
        {
            writeC(0x28);
            writeD(_id);
            writeD(_x);
            writeD(_y);
            writeD(_z);
        }
    }
}
