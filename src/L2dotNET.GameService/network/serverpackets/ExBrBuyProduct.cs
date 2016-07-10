namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExBrBuyProduct : GameServerNetworkPacket
    {
        private readonly int _result;

        public ExBrBuyProduct(int result)
        {
            _result = result;
        }

        protected internal override void Write()
        {
            WriteC(0xFE);
            WriteH(0xCC);
            WriteD(_result);
        }
    }
}