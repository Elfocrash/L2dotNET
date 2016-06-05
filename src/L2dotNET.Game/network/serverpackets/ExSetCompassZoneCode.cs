
namespace L2dotNET.GameService.network.l2send
{
    class ExSetCompassZoneCode : GameServerNetworkPacket
    {
        public static int ALTEREDZONE       = 0x08;
        public static int SIEGEWARZONE1     = 0x0A;
        public static int SIEGEWARZONE2     = 0x0B;
        public static int PEACEZONE         = 0x0C;
        public static int SEVENSIGNSZONE    = 0x0D;
        public static int PVPZONE           = 0x0E;
        public static int GENERALZONE       = 0x0F;

        private readonly int ZoneCode;
        public ExSetCompassZoneCode(int type)
        {
            ZoneCode = type;
        }

        protected internal override void write()
        {
            writeC(0xFE);
            writeH(0x33);
            writeD(ZoneCode);
        }
    }
}
