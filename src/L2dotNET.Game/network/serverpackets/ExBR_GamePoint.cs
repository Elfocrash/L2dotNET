
namespace L2dotNET.Game.network.l2send
{
    class ExBR_GamePoint : GameServerNetworkPacket
    {
        private int id;
        private long points;
        public ExBR_GamePoint(int id, long points)
        {
            this.id = id;
            this.points = points;
        }

        protected internal override void write()
        {
            writeC(0xFE);
            writeH(0xC9);

            writeD(id);
            writeQ(points);
            writeD(0);
        }
    }
}
