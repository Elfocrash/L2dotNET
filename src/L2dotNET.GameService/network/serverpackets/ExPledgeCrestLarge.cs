using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExPledgeCrestLarge : GameserverPacket
    {
        private readonly int _id;
        private readonly byte[] _picture;

        public ExPledgeCrestLarge(int id, byte[] picture)
        {
            _id = id;
            if (picture == null)
                picture = new byte[0];

            _picture = picture;
        }

        public override void Write()
        {
            WriteByte(0xfe);
            WriteShort(0x1b);

            WriteInt(0x00); //???
            WriteInt(_id);
            WriteInt(_picture.Length);
            WriteBytesArray(_picture);
        }
    }
}