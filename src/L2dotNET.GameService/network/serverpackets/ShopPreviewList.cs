using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Tables;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ShopPreviewList : GameServerNetworkPacket
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
            WriteC(0xef);
            WriteC(0xc0); // ?
            WriteC(0x13); // ?
            WriteC(0x00); // ?
            WriteC(0x00); // ?
            WriteD(_adena); // current money
            WriteD(_shopId);

            foreach (NdShopItem si in _shop.Items)
            {
                WriteD(si.Item.ItemId);
                WriteH(si.Item.Type2());
                WriteH(si.Item.BodyPartId());
                WriteD(10);
            }
        }
    }
}