namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExRaidCharacterSelected : GameserverPacket
    {
        private int _id;

        public ExRaidCharacterSelected(int id)
        {
            _id = id;
        }

        protected internal override void Write()
        {
            WriteByte(0xFE);
            WriteShort(0xBA);

            //  writeD(id);
            //  writeQ(0);
            //  writeD(0);
        }
    }
}