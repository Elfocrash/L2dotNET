using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExBrBuyProduct : GameserverPacket
    {
        private readonly int _result;

        public ExBrBuyProduct(int result)
        {
            _result = result;
        }

        public override void Write()
        {
            WriteByte(0xFE);
            WriteShort(0xCC);
            WriteInt(_result);
        }
    }
}