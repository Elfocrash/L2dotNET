using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ChangeWaitType : GameServerNetworkPacket
    {
        private readonly int sId;
        private readonly int type;
        private readonly int x;
        private readonly int y;
        private readonly int z;
        public static int SIT = 0;
        public static int STAND = 1;
        public static int FAKE = 2;
        public static int FAKE_STOP = 3;

        public ChangeWaitType(L2Player player, int type)
        {
            sId = player.ObjID;
            x = player.X;
            y = player.Y;
            z = player.Z;
            this.type = type;
        }

        protected internal override void write()
        {
            writeC(0x2f);
            writeD(sId);
            writeD(type);
            writeD(x);
            writeD(y);
            writeD(z);
        }
    }
}