using L2dotNET.GameService.tables;

namespace L2dotNET.GameService.network.l2send
{
    class ShopPreviewList : GameServerNetworkPacket
    {
        private readonly long _adena;
        private readonly ND_shopList _shop;
        private readonly int _shopId;

        public ShopPreviewList(L2Player player, ND_shopList shop, int shopId)
        {
            _adena = player.getAdena();
            _shop = shop;
            _shopId = shopId;
        }

        protected internal override void write()
        {
            writeC(0xef);
            writeC(0xc0); // ?
            writeC(0x13); // ?
            writeC(0x00); // ?
            writeC(0x00); // ?
            writeD(_adena); // current money
            writeD(_shopId);

            foreach (ND_shopItem si in _shop.items)
            {
                writeD(si.item.ItemID);
                writeH(si.item.Type2());
                writeH(si.item.BodyPartId());
                writeD(10);
            }
        }
    }
}