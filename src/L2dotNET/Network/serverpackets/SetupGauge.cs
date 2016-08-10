namespace L2dotNET.Network.serverpackets
{
    class SetupGauge : GameserverPacket
    {
        public enum SgColor
        {
            Blue = 0,
            Red = 1,
            Cyan = 2,
            Green = 3
        }

        private readonly SgColor _color;
        private readonly int _time;
        private int _id;

        public SetupGauge(int id, SgColor color, int time)
        {
            _id = id;
            _color = color;
            _time = time;
        }

        public override void Write()
        {
            WriteByte(0x6d);
            //writeD(_id);
            WriteInt((int)_color);
            WriteInt(_time);
            WriteInt(_time); //c2
        }
    }
}