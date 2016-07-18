namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExSetCompassZoneCode : GameserverPacket
    {
        public static int Alteredzone = 0x08;
        public static int Siegewarzone1 = 0x0A;
        public static int Siegewarzone2 = 0x0B;
        public static int Peacezone = 0x0C;
        public static int Sevensignszone = 0x0D;
        public static int Pvpzone = 0x0E;
        public static int Generalzone = 0x0F;

        private readonly int _zoneCode;

        public ExSetCompassZoneCode(int type)
        {
            _zoneCode = type;
        }

        protected internal override void Write()
        {
            WriteByte(0xFE);
            WriteShort(0x33);
            WriteInt(_zoneCode);
        }
    }
}