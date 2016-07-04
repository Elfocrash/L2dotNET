namespace L2dotNET.GameService.Network.Serverpackets
{
    class StartRotation : GameServerNetworkPacket
    {
        private readonly int _sId;
        private readonly int _degree;
        private readonly int _side;
        private readonly int _speed;

        public StartRotation(int sId, int degree, int side, int speed)
        {
            this._sId = sId;
            this._degree = degree;
            this._side = side;
            this._speed = speed;
        }

        protected internal override void Write()
        {
            WriteC(0x62);
            WriteD(_sId);
            WriteD(_degree);
            WriteD(_side);
            WriteD(_speed);
        }
    }
}