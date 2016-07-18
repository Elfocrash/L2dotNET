using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Tables;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExBuySellListBuy : GameserverPacket
    {
        private readonly NdShopList _shop;
        private readonly int _adena;
        private readonly double _mod;
        private readonly double _tax;
        private readonly int _shopId;

        public ExBuySellListBuy(L2Player player, NdShopList shop, double mod, double tax, int shopId)
        {
            _shop = shop;
            _adena = player.GetAdena();
            _mod = mod;
            _tax = tax;
            _shopId = shopId;
        }

        public ExBuySellListBuy(int adena)
        {
            _adena = adena;
        }

        public override void Write()
        {
            WriteByte(0xFE);
            WriteShort(0xB7);
            WriteInt(0);
            WriteLong(_adena);
            WriteInt(_shopId);

            if (_shop == null)
            {
                WriteShort(0);
                return;
            }

            WriteShort(_shop.Items.Count);
            foreach (NDShopItem si in _shop.Items)
            {
                WriteInt(0); //objectId
                WriteInt(si.Item.ItemId);
                WriteInt(0);
                WriteLong(si.Count < 0 ? 0 : si.Count);
                WriteShort(si.Item.Type2);
                WriteShort(0);
                WriteShort(0);
                WriteInt(si.Item.BodyPart);

                WriteShort(0);
                WriteShort(0);
                WriteInt(0);
                WriteInt(0);
                WriteInt(-9999);

                // Enchant Effects
                WriteShort(0x00);
                WriteShort(0x00);
                WriteShort(0x00);

                WriteLong((long)(si.Item.ReferencePrice * _mod * _tax));
            }
        }
    }
}