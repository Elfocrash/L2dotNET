namespace L2dotNET.GameService.Network.Serverpackets
{
    class StartRotation : GameserverPacket
    {
        private readonly int _sId;
        private readonly int _degree;
        private readonly int _side;
        private readonly int _speed;

        public StartRotation(int sId, int degree, int side, int speed)
        {
            _sId = sId;
            _degree = degree;
            _side = side;
            _speed = speed;
        }

        protected internal override void Write()
        {
            WriteByte(0x62);
            WriteInt(_sId);
            WriteInt(_degree);
            WriteInt(_side);
            WriteInt(_speed);
        }
    }
}