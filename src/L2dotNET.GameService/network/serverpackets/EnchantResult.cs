namespace L2dotNET.GameService.Network.Serverpackets
{
    class EnchantResult : GameServerNetworkPacket
    {
        private readonly EnchantResultVal _result;
        private int _crystal;
        private long _count;

        public EnchantResult(EnchantResultVal result, int crystal = 0, long count = 0)
        {
            _result = result;
            _crystal = crystal;
            _count = count;
        }

        protected internal override void Write()
        {
            WriteC(0x81);
            WriteD((int)_result);
            //writeD(crystal);
            //writeQ(count);
        }
    }
}