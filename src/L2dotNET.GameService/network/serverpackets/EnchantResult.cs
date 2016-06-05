namespace L2dotNET.GameService.Network.Serverpackets
{
    class EnchantResult : GameServerNetworkPacket
    {
        private readonly EnchantResultVal result;
        private int crystal;
        private long count;

        public EnchantResult(EnchantResultVal result, int crystal = 0, long count = 0)
        {
            this.result = result;
            this.crystal = crystal;
            this.count = count;
        }

        protected internal override void write()
        {
            writeC(0x81);
            writeD((int)result);
            //writeD(crystal);
            //writeQ(count);
        }
    }
}