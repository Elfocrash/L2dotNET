namespace L2dotNET.GameService.network.l2send
{
    class VehicleStarted : GameServerNetworkPacket
    {
        private readonly int sId;
        private readonly int type;

        public VehicleStarted(int sId, int type)
        {
            this.sId = sId;
            this.type = type;
        }

        protected internal override void write()
        {
            writeC(0xBA);
            writeD(sId);
            writeD(type);
        }
    }
}