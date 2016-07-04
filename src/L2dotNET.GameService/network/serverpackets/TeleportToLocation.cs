namespace L2dotNET.GameService.Network.Serverpackets
{
    class TeleportToLocation : GameServerNetworkPacket
    {
        private readonly int _x;
        private readonly int _y;
        private readonly int _z;
        private readonly int _id;
        private int _heading;

        public TeleportToLocation(int id, int x, int y, int z, int h)
        {
            _x = x;
            _y = y;
            _z = z;
            _id = id;
            _heading = h;
        }

        protected internal override void Write()
        {
            WriteC(0x28);
            WriteD(_id);
            WriteD(_x);
            WriteD(_y);
            WriteD(_z);
        }
    }
}