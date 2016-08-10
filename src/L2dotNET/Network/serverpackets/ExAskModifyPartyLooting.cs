namespace L2dotNET.Network.serverpackets
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

        public override void Write()
        {
            WriteByte(0xFE);
            WriteShort(0xBE);
            WriteString(_leader);
            WriteInt(_mode);
        }
    }
}