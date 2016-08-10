namespace L2dotNET.Network.serverpackets
{
    class GetItem : GameserverPacket
    {
        private readonly int _id;
        private readonly int _itemId;
        private readonly int _x;
        private readonly int _y;
        private readonly int _z;

        public GetItem(int id, int itemId, int x, int y, int z)
        {
            _id = id;
            _itemId = itemId;
            _x = x;
            _y = y;
            _z = z;
        }

        public override void Write()
        {
            WriteByte(0x0d);
            WriteInt(_id);
            WriteInt(_itemId);
            WriteInt(_x);
            WriteInt(_y);
            WriteInt(_z);
        }
    }
}