namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExBR_BuyProduct : GameServerNetworkPacket
    {
        private readonly int result;

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