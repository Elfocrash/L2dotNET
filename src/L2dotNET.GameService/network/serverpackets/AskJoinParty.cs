namespace L2dotNET.GameService.Network.Serverpackets
{
    class AskJoinParty : GameServerNetworkPacket
    {
        private readonly string _asker;
        private readonly int _itemDistribution;

        public AskJoinParty(string asker, int itemDistribution)
        {
            _asker = asker;
            _itemDistribution = itemDistribution;
        }

        protected internal override void Write()
        {
            WriteC(0x39);
            WriteS(_asker);
            WriteD(_itemDistribution);
        }
    }
}