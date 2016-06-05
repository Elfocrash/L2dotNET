namespace L2dotNET.GameService.network.serverpackets
{
    class SendTradeRequest : GameServerNetworkPacket
    {
        private readonly int sId;

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