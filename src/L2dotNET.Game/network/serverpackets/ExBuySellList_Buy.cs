using L2dotNET.GameService.tables;

namespace L2dotNET.GameService.network.l2send
{
    class ExBuySellList_Buy : GameServerNetworkPacket
    {
        private readonly ND_shopList _shop;
        private readonly long _adena;
        private readonly double _mod;
        private readonly double _tax;
        private readonly int _shopId;

        public ExBuySellList_Buy(L2Player player, ND_shopList shop, double mod, double tax, int shopId)
        {
            _shop = shop;
            _adena = player.getAdena();
            _mod = mod;
            _tax = tax;
            _shopId = shopId;
        }

        public ExBuySellList_Buy(long adena)
        {
            _adena = adena;
        }

        protected internal override void write()
        {
            writeC(0xFE);
            writeH(0xB7);
            writeD(0);
            writeQ(_adena);
            writeD(_shopId);

            if (_shop == null)
            {
                writeH(0);
                return;
            }

            writeH(_shop.items.Count);
            foreach (ND_shopItem si in _shop.items)
            {
                writeD(0); //objectId
                writeD(si.item.ItemID);
                writeD(0);
                writeQ(si.count < 0 ? 0 : si.count);
                writeH(si.item.Type2());
                writeH(0);
                writeH(0);
                writeD(si.item.BodyPartId());

                writeH(0);
                writeH(0);
                writeD(0);
                writeD(si.item.Durability);
                writeD(-9999);

                writeH(si.item.BaseAttrAttackType);
                writeH(si.item.BaseAttrAttackValue);
                writeH(si.item.BaseAttrDefenseValueFire);
                writeH(si.item.BaseAttrDefenseValueWater);
                writeH(si.item.BaseAttrDefenseValueWind);
                writeH(si.item.BaseAttrDefenseValueEarth);
                writeH(si.item.BaseAttrDefenseValueHoly);
                writeH(si.item.BaseAttrDefenseValueUnholy);

                // Enchant Effects
                writeH(0x00);
                writeH(0x00);
                writeH(0x00);

                writeQ((long)(si.item.Price * _mod * _tax));
            }
        }
    }
}
