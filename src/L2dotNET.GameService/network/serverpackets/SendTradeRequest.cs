namespace L2dotNET.GameService.Network.Serverpackets
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