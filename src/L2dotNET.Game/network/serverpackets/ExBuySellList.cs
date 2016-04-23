using c_game.tables;
using c_game.model.items;
using System.Collections.Generic;

namespace c_game.network.l2send
{
    class ExBuySellList : SendBasePacket
    {
        private ND_shopList _shop;
        private long _adena;
        private double _mod;
        private double _tax;
        private int _shopId;
        //List<L2Item> _sells = new List<L2Item>();

        public ExBuySellList(L2Player player, ND_shopList shop, double mod, double tax, int shopId)
        {
            base.makeme();
            _shop = shop;
            _adena = player.getAdena();
            _mod = mod;
            _tax = tax;
            _shopId = shopId;

            //foreach (L2Item item in player.getAllItems())
            //{
            //    if (item._template.is_trade == 0 || item._augmentationId > 0 || item._isEquipped == 1)
            //        continue;

            //    if (item._template._type == ItemTemplate.L2ItemType.asset)
            //        continue;

            //    _sells.Add(item);
            //}
        }

        protected internal override void write()
        {
            writeC(0xFE);
            writeH(0xB7);
            writeD(0x00); // Freya Unknown
            writeQ(_adena);
            writeD(_shopId);
            writeH(_shop.items.Count);

            foreach (ND_shopItem si in _shop.items)
            {
                writeD(0x00); //objectId
                writeD(si.item._itemId);
                writeQ(si.count == -1 ? 0 : si.count);
                writeD(0x00); // Enchant effect 1
                writeH(si.item.getType2());
                writeD(0x00); // Freya Unknown
                writeD(si.item.getBodyPartId());

                writeH(0x00); // item enchant level
                writeH(0x00); // ?
                writeH(0x00);

                writeH(0x00); // Freya Unknown

                writeD(-1);
                writeH(0xD8F1); // Freya Unknown
                writeH(0xFFFF); // Freya Unknown
                writeH(0xFEFF);

                // T1
                for (byte i = 0; i < 7; i++)
                    writeH(0x00);

                writeH(0x00); // Freya Unknown
                writeH(0x00); // Freya Unknown
                writeH(0x00); // Freya Unknown
                writeQ((long)(si.item._price * _mod * _tax));
            }
        }
    }
}
