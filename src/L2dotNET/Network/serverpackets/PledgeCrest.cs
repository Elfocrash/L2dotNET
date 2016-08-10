namespace L2dotNET.Network.serverpackets
{
    class PledgeCrest : GameserverPacket
    {
        private readonly int _id;
        private readonly byte[] _picture;

        public PledgeCrest(int id, byte[] picture)
        {
            _id = id;
            if (picture == null)
                picture = new byte[0];

            _picture = picture;
        }

        public override void Write()
        {
            WriteByte(0x6a);
            WriteInt(_id);
            WriteInt(_picture.Length);
            WriteBytesArray(_picture);
        }
    }
}