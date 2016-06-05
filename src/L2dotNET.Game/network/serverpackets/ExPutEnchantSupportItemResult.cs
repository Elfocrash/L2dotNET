namespace L2dotNET.GameService.network.serverpackets
{
    class ExPutEnchantSupportItemResult : GameServerNetworkPacket
    {
        private readonly int result;

        public ExPutEnchantSupportItemResult(int result = 0)
        {
            this.result = result;
        }

        protected internal override void write()
        {
            writeC(0xfe);
            writeH(0x82);
            writeD(result);
        }
    }
}