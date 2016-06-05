namespace L2dotNET.GameService.network.l2send
{
    class ExRaidCharacterSelected : GameServerNetworkPacket
    {
        private int id;

        public ExRaidCharacterSelected(int id)
        {
            this.id = id;
        }

        protected internal override void write()
        {
            writeC(0xFE);
            writeH(0xBA);

            //  writeD(id);
            //  writeQ(0);
            //  writeD(0);
        }
    }
}