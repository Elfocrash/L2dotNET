namespace L2dotNET.GameService.network.l2send
{
    class StartRotation : GameServerNetworkPacket
    {
        private readonly int sId;
        private readonly int degree;
        private readonly int side;
        private readonly int speed;

        public StartRotation(int sId, int degree, int side, int speed)
        {
            this.sId = sId;
            this.degree = degree;
            this.side = side;
            this.speed = speed;
        }

        protected internal override void write()
        {
            writeC(0x62);
            writeD(sId);
            writeD(degree);
            writeD(side);
            writeD(speed);
        }
    }
}