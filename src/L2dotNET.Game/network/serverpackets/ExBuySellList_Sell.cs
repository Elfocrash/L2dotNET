using System.Collections.Generic;
using L2dotNET.GameService.model.items;

namespace L2dotNET.GameService.network.l2send
{
    class ExBuySellList_Sell : GameServerNetworkPacket
    {
        List<L2Item> _sells = new List<L2Item>(), _refund = new List<L2Item>();
        public ExBuySellList_Sell(L2Player player)
        {
            foreach (L2Item item in player.getAllItems())
            {
                if (item.NotForTrade())
                    continue;

                _sells.Add(item);
            }

            _refund = player.Refund._items;
        }

        protected internal override void write()
        {
            writeC(0xFE);
            writeH(0xB7);
            writeD(1);
            writeH(_sells.Count);

            foreach (L2Item item in _sells)
            {
                writeD(item.ObjID);
                writeD(item.Template.ItemID);
                writeD(0);
                writeQ(item.Count);
                writeH(item.Template.Type2());
                writeH(item.CustomType1);
                writeH(0);
                writeD(item.Template.BodyPartId());
                writeH(item.Enchant);
                writeH(item.CustomType2);
                writeD(item.AugmentationID);
                writeD(item.Durability);
                writeD(item.LifeTimeEnd());

                writeH(item.AttrAttackType);
                writeH(item.AttrAttackValue);
                writeH(item.AttrDefenseValueFire);
                writeH(item.AttrDefenseValueWater);
                writeH(item.AttrDefenseValueWind);
                writeH(item.AttrDefenseValueEarth);
                writeH(item.AttrDefenseValueHoly);
                writeH(item.AttrDefenseValueUnholy);

                writeH(item.Enchant1);
                writeH(item.Enchant2);
                writeH(item.Enchant3);

                writeQ(item.Template.Price / 2);
            }

            writeH(_refund.Count);

            int idx = 0;
            foreach (L2Item item in _refund)
            {
                writeD(item.ObjID);
                writeD(item.Template.ItemID);
                writeD(0);
                writeQ(item.Count);
                writeH(item.Template.Type2());
                writeH(item.CustomType1);
                writeH(0);
                writeD(item.Template.BodyPartId());
                writeH(item.Enchant);
                writeH(item.CustomType2);
                writeD(item.AugmentationID);
                writeD(item.Durability);
                writeD(item.LifeTimeEnd());

                writeH(item.AttrAttackType);
                writeH(item.AttrAttackValue);
                writeH(item.AttrDefenseValueFire);
                writeH(item.AttrDefenseValueWater);
                writeH(item.AttrDefenseValueWind);
                writeH(item.AttrDefenseValueEarth);
                writeH(item.AttrDefenseValueHoly);
                writeH(item.AttrDefenseValueUnholy);

                writeH(item.Enchant1);
                writeH(item.Enchant2);
                writeH(item.Enchant3);

                writeD(idx++);
                writeQ(item.Template.Price / 2 * item.Count);
            }

            writeC(0);
        }
    }
}
