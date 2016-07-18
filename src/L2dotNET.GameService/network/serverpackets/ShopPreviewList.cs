using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Tables;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ShopPreviewList : GameserverPacket
    {
        private readonly int _adena;
        private readonly NdShopList _shop;
        private readonly int _shopId;

        public ShopPreviewList(L2Player player, NdShopList shop, int shopId)
        {
            _adena = player.GetAdena();
            _shop = shop;
            _shopId = shopId;
        }

        protected internal override void Write()
        {
            WriteByte(0xef);
            WriteByte(0xc0); // ?
            WriteByte(0x13); // ?
            WriteByte(0x00); // ?
            WriteByte(0x00); // ?
            WriteInt(_adena); // current money
            WriteInt(_shopId);

            foreach (NDShopItem si in _shop.Items)
            {
                WriteInt(si.Item.ItemId);
                WriteShort(si.Item.Type2);
                WriteShort(si.Item.BodyPart);
                WriteInt(10);
            }
        }
    }
}