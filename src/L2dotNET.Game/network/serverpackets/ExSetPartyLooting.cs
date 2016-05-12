
namespace L2dotNET.GameService.network.l2send
{
    class ExSetPartyLooting : GameServerNetworkPacket
    {
        private int result = 0;
        private int mode = 0;

        public ExSetPartyLooting(short VoteId)
        {
            if (VoteId != -1)
            {
                result = 1;
                mode = VoteId;
            }
        }

        protected internal override void write()
        {
            writeC(0xFE);
            writeH(0xBF);
            writeD(result);
            writeD(mode);
        }
    }
}
