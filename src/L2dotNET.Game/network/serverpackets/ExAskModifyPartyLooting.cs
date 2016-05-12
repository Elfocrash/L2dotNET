
namespace L2dotNET.GameService.network.l2send
{
    class ExAskModifyPartyLooting : GameServerNetworkPacket
    {
        private string leader;
        private byte mode;
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
