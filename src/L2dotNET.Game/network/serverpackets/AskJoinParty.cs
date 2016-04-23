
namespace L2dotNET.Game.network.l2send
{
    class AskJoinParty : GameServerNetworkPacket
    {
        private string asker;
        private int itemDistribution;

        public AskJoinParty(string asker, int itemDistribution)
        {
            this.asker = asker;
            this.itemDistribution = itemDistribution;
        }

        protected internal override void write()
        {
            writeC(0x39);
            writeS(asker);
            writeD(itemDistribution);
        }
    }
}
