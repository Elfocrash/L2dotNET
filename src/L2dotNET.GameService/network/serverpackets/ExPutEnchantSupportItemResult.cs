namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExPutEnchantSupportItemResult : GameServerNetworkPacket
    {
        private readonly int _result;

        public ExPutEnchantSupportItemResult(int result = 0)
        {
            this._result = result;
        }

        protected internal override void Write()
        {
            WriteC(0xfe);
            WriteH(0x82);
            WriteD(_result);
        }
    }
}