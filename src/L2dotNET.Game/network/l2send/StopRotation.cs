
namespace L2dotNET.Game.network.l2send
{
    class StopRotation : GameServerNetworkPacket
    {
        private int sId;
        private int degree;
        private int speed;
        public StopRotation(int sId, int degree, int speed)
        {
            this.sId = sId;
            this.degree = degree;
            this.speed = speed;
        }

        protected internal override void write()
        {
            writeC(0x61);
            writeD(sId);
            writeD(degree);
            writeD(speed);
            writeC(0); // ?
        }
    }
}
