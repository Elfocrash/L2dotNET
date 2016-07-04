namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExSetPartyLooting : GameServerNetworkPacket
    {
        private readonly int _result;
        private readonly int _mode;

        public ExSetPartyLooting(short voteId)
        {
            if (voteId != -1)
            {
                _result = 1;
                _mode = voteId;
            }
        }

        protected internal override void Write()
        {
            WriteC(0xFE);
            WriteH(0xBF);
            WriteD(_result);
            WriteD(_mode);
        }
    }
}