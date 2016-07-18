namespace L2dotNET.GameService.Network.Serverpackets
{
    class StopRotation : GameserverPacket
    {
        private readonly int _sId;
        private readonly int _degree;
        private readonly int _speed;

        public StopRotation(int sId, int degree, int speed)
        {
            _sId = sId;
            _degree = degree;
            _speed = speed;
        }

        protected internal override void Write()
        {
            WriteByte(0x63);
            WriteInt(_sId);
            WriteInt(_degree);
            WriteInt(_speed);
            WriteByte(_degree);
        }
    }
}