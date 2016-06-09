namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExSetPartyLooting : GameServerNetworkPacket
    {
        private readonly int result;
        private readonly int mode;

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