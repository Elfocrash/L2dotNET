using L2dotNET.model.player;
using L2dotNET.tables;

namespace L2dotNET.Network.serverpackets
{
    class ShopPreviewList : GameserverPacket
    {
        private readonly int _adena;
        private readonly int _shopId;

        public ShopPreviewList(L2Player player, int shopId)
        {
            _adena = player.GetAdena();
            _shopId = shopId;
        }

        public override void Write()
        {
            WriteByte(0xef);
            WriteByte(0xc0); // ?
            WriteByte(0x13); // ?
            WriteByte(0x00); // ?
            WriteByte(0x00); // ?
            WriteInt(_adena); // current money
            WriteInt(_shopId);

            //foreach (NDShopItem si in _shop.Items)
            //{
            //    WriteInt(si.Item.ItemId);
            //    WriteShort(si.Item.Type2);
            //    WriteShort(si.Item.BodyPart);
            //    WriteInt(10);
            //}
        }
    }
}