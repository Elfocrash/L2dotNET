namespace L2dotNET.Network.serverpackets
{
    class AskJoinParty : GameserverPacket
    {
        private readonly string _asker;
        private readonly int _itemDistribution;

        public AskJoinParty(string asker, int itemDistribution)
        {
            _asker = asker;
            _itemDistribution = itemDistribution;
        }

        public override void Write()
        {
            WriteByte(0x39);
            WriteString(_asker);
            WriteInt(_itemDistribution);
        }
    }
}