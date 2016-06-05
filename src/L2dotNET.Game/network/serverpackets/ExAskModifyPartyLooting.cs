namespace L2dotNET.GameService.network.l2send
{
    class ExAskModifyPartyLooting : GameServerNetworkPacket
    {
        private readonly string leader;
        private readonly byte mode;

        public ExAskModifyPartyLooting(string leader, byte mode)
        {
            this.leader = leader;
            this.mode = mode;
        }

        protected internal override void write()
        {
            writeC(0xFE);
            writeH(0xBE);
            writeS(leader);
            writeD(mode);
        }
    }
}