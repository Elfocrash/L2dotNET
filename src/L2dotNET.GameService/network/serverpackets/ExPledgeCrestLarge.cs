namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExPledgeCrestLarge : GameServerNetworkPacket
    {
        private readonly int _id;
        private readonly byte[] _picture;

        public ExPledgeCrestLarge(int id, byte[] picture)
        {
            this._id = id;
            if (picture == null)
                picture = new byte[0];

            this._picture = picture;
        }

        protected internal override void Write()
        {
            WriteC(0xfe);
            WriteH(0x1b);

            WriteD(0x00); //???
            WriteD(_id);
            WriteD(_picture.Length);
            WriteB(_picture);
        }
    }
}