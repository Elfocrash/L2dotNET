namespace L2dotNET.GameService.network.serverpackets
{
    class ExPutEnchantTargetItemResult : GameServerNetworkPacket
    {
        private readonly int result;

        public ExPutEnchantTargetItemResult(int result = 0)
        {
            this.result = result;
        }

        protected internal override void write()
        {
            writeC(0xfe);
            writeH(0x81);
            writeD(result);
        }
    }
}