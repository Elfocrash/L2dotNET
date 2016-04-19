using L2dotNET.Game.model.items;

namespace L2dotNET.Game.network.l2send
{
    class GetItem : GameServerNetworkPacket
    {
        private int _id;
        private int _itemId;
        private int _x;
        private int _y;
        private int _z;
        public GetItem(int id, int itemId, int x, int y, int z)
        {
            _id = id;
            _itemId = itemId;
            _x =  x;
            _y = y;
            _z = z;
        }

        protected internal override void write()
        {
            writeC(0x17);
            writeD(_id);
            writeD(_itemId);
            writeD(_x);
            writeD(_y);
            writeD(_z);
        }
    }
}
