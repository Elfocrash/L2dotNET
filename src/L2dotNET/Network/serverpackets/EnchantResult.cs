namespace L2dotNET.Network.serverpackets
{
    class EnchantResult : GameserverPacket
    {
        private readonly EnchantResultVal _result;
        private int _crystal;
        private int _count;

        public EnchantResult(EnchantResultVal result, int crystal = 0, int count = 0)
        {
            _result = result;
            _crystal = crystal;
            _count = count;
        }

        public override void Write()
        {
            WriteByte(0x81);
            WriteInt((int)_result);
            //writeD(crystal);
            //writeQ(count);
        }
    }
}