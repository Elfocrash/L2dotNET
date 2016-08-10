namespace L2dotNET.Network.serverpackets
{
    class ExPutEnchantSupportItemResult : GameserverPacket
    {
        private readonly int _result;

        public ExPutEnchantSupportItemResult(int result = 0)
        {
            _result = result;
        }

        public override void Write()
        {
            WriteByte(0xfe);
            WriteShort(0x82);
            WriteInt(_result);
        }
    }
}