using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ChangeWaitType : GameServerNetworkPacket
    {
        private readonly int _sId;
        private readonly int _type;
        private readonly int _x;
        private readonly int _y;
        private readonly int _z;
        public static int Sit = 0;
        public static int Stand = 1;
        public static int Fake = 2;
        public static int FakeStop = 3;

        public ChangeWaitType(L2Player player, int type)
        {
            _sId = player.ObjId;
            _x = player.X;
            _y = player.Y;
            _z = player.Z;
            _type = type;
        }

        protected internal override void Write()
        {
            WriteC(0x2f);
            WriteD(_sId);
            WriteD(_type);
            WriteD(_x);
            WriteD(_y);
            WriteD(_z);
        }
    }
}