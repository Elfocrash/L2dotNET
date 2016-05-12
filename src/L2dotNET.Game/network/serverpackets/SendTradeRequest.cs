
namespace L2dotNET.GameService.network.l2send
{
    class SendTradeRequest : GameServerNetworkPacket
    {
        private int sId;
        public SendTradeRequest(int sId)
        {
            this.sId = sId;
        }

        protected internal override void write()
        {
            writeC(0x5e);
            writeD(sId);
        }
    }
}
