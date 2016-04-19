
namespace L2dotNET.Game.network.l2send
{
    class ChangeWaitType : GameServerNetworkPacket
    {
        private int sId;
        private int type;
        private int x;
        private int y;
        private int z;
        public static int SIT = 0;
        public static int STAND = 1;
        public static int FAKE = 2;
        public static int FAKE_STOP = 3;

        public ChangeWaitType(L2Player player, int type)
        {
            this.sId = player.ObjID;
            this.x = player.X;
            this.y = player.Y;
            this.z = player.Z;
            this.type = type;
        }

        protected internal override void write()
        {
            writeC(0x29);
            writeD(sId);
            writeD(type);
            writeD(x);
            writeD(y);
            writeD(z);
        }
    }
}
