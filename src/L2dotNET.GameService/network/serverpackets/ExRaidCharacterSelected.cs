namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExRaidCharacterSelected : GameServerNetworkPacket
    {
        private int _id;

        public ExRaidCharacterSelected(int id)
        {
            _id = id;
        }

        protected internal override void Write()
        {
            WriteC(0xFE);
            WriteH(0xBA);

            //  writeD(id);
            //  writeQ(0);
            //  writeD(0);
        }
    }
}