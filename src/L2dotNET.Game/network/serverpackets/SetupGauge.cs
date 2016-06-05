namespace L2dotNET.GameService.network.serverpackets
{
    class SetupGauge : GameServerNetworkPacket
    {
        public enum SG_color
        {
            blue = 0,
            red = 1,
            cyan = 2,
            green = 3
        }

        private readonly SG_color _color;
        private readonly int _time;
        private int _id;

        public SetupGauge(int id, SG_color color, int time)
        {
            _id = id;
            _color = color;
            _time = time;
        }

        protected internal override void write()
        {
            writeC(0x6d);
            //writeD(_id);
            writeD((int)_color);
            writeD(_time);
            writeD(_time); //c2
        }
    }
}