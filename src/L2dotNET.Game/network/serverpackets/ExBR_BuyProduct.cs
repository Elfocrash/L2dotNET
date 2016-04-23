
namespace L2dotNET.Game.network.l2send
{
    class ExBR_BuyProduct : GameServerNetworkPacket
    {
        private int result;
        public ExBR_BuyProduct(int result)
        {
            this.result = result;
        }

        protected internal override void write()
        {
            writeC(0xFE);
            writeH(0xCC);
            writeD(result);
        }
    }
}
