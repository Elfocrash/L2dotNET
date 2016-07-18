using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class SendTradeRequest : GameserverPacket
    {
        private readonly int _sId;

        public SendTradeRequest(int sId)
        {
            _sId = sId;
        }

        public override void Write()
        {
            WriteByte(0x5e);
            WriteInt(_sId);
        }
    }
}