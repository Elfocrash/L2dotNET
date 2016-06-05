
namespace L2dotNET.GameService.network.l2send
{
    class StopRotation : GameServerNetworkPacket
    {
        private readonly int sId;
        private readonly int degree;
        private readonly int speed;
        public StopRotation(int sId, int degree, int speed)
        {
            this.sId = sId;
            this.degree = degree;
            this.speed = speed;
        }

        protected internal override void write()
        {
            writeC(0x63);
            writeD(sId);
            writeD(degree);
            writeD(speed);
            writeC(degree);
        }
    }
}
