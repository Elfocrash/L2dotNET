
namespace L2dotNET.Game.network.l2send
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
            writeC(0x70);
            writeD(sId);
        }
    }
}
