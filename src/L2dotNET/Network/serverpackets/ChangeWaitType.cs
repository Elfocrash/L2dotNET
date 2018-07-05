using L2dotNET.Models.Player;

namespace L2dotNET.Network.serverpackets
{
    class ChangeWaitType : GameserverPacket
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
            _sId = player.CharacterId;
            _x = player.X;
            _y = player.Y;
            _z = player.Z;
            _type = type;
        }

        public override void Write()
        {
            WriteByte(0x2f);
            WriteInt(_sId);
            WriteInt(_type);
            WriteInt(_x);
            WriteInt(_y);
            WriteInt(_z);
        }
    }
}