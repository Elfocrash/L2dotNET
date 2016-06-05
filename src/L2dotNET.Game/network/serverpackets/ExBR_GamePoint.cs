namespace L2dotNET.GameService.network.serverpackets
{
    class ExBR_GamePoint : GameServerNetworkPacket
    {
        private readonly int id;
        private readonly long points;

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