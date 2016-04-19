
namespace L2dotNET.Game.network.l2send
{
    class VehicleStarted : GameServerNetworkPacket
    {
        private int sId;
        private int type;
        public VehicleStarted(int sId, int type)
        {
            this.sId = sId;
            this.type = type;
        }

        protected internal override void write()
        {
            writeC(0xC0);
            writeD(sId);
            writeD(type);
        }
    }
}
