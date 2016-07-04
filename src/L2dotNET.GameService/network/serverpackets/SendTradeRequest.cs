namespace L2dotNET.GameService.Network.Serverpackets
{
    class SendTradeRequest : GameServerNetworkPacket
    {
        private readonly int _sId;

        public SendTradeRequest(int sId)
        {
            this._sId = sId;
        }

        protected internal override void Write()
        {
            WriteC(0x5e);
            WriteD(_sId);
        }
    }
}