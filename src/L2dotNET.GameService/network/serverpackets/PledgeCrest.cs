namespace L2dotNET.GameService.Network.Serverpackets
{
    class PledgeCrest : GameServerNetworkPacket
    {
        private readonly int _id;
        private readonly byte[] _picture;

        public PledgeCrest(int id, byte[] picture)
        {
            this._id = id;
            if (picture == null)
            {
                picture = new byte[0];
            }

            this._picture = picture;
        }

        protected internal override void Write()
        {
            WriteC(0x6a);
            WriteD(_id);
            WriteD(_picture.Length);
            WriteB(_picture);
        }
    }
}