
namespace L2dotNET.Game.network.l2send
{
    class ExBrPremiumState : GameServerNetworkPacket
    {
        private int sid;
        private byte status;
        public ExBrPremiumState(int sid, byte status)
        {
            this.sid = sid;
            this.status = status;
        }

        protected internal override void write()
        {
            writeC(0xFE);
            writeH(0xCD);
            writeD(sid);
            writeC(status);
        }
    }
}
