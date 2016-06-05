namespace L2dotNET.GameService.network.serverpackets
{
    class PledgeCrest : GameServerNetworkPacket
    {
        private readonly int id;
        private readonly byte[] picture;

        public PledgeCrest(int id, byte[] picture)
        {
            this.id = id;
            if (picture == null)
                picture = new byte[0];

            this.picture = picture;
        }

        protected internal override void write()
        {
            writeC(0x6a);
            writeD(id);
            writeD(picture.Length);
            writeB(picture);
        }
    }
}