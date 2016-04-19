
namespace L2dotNET.Game.network.l2send
{
    class ExPutEnchantSupportItemResult : GameServerNetworkPacket
    {
        private int result;
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
