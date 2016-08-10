namespace L2dotNET.Network.serverpackets
{
    class ExRaidCharacterSelected : GameserverPacket
    {
        private int _id;

        public ExRaidCharacterSelected(int id)
        {
            _id = id;
        }

        public override void Write()
        {
            WriteByte(0xFE);
            WriteShort(0xBA);

            //  writeD(id);
            //  writeQ(0);
            //  writeD(0);
        }
    }
}