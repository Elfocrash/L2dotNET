namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExAskModifyPartyLooting : GameserverPacket
    {
        private readonly string _leader;
        private readonly byte _mode;

        public ExAskModifyPartyLooting(string leader, byte mode)
        {
            _leader = leader;
            _mode = mode;
        }

        protected internal override void Write()
        {
            WriteByte(0xFE);
            WriteShort(0xBE);
            WriteString(_leader);
            WriteInt(_mode);
        }
    }
}