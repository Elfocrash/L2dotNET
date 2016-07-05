using System.Collections.Generic;
using System.Linq;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExBuySellListSell : GameServerNetworkPacket
    {
        private readonly List<L2Item> _sells = new List<L2Item>();
        private readonly List<L2Item> _refund;

        public ExBuySellListSell(L2Player player)
        {
            foreach (L2Item item in player.GetAllItems().Where(item => !item.NotForTrade()))
                _sells.Add(item);

           // _refund = player.Refund._items;
        }

        protected internal override void Write()
        {
            WriteC(0xFE);
            WriteH(0xB7);
            WriteD(1);
            WriteH(_sells.Count);

            foreach (L2Item item in _sells)
            {
                WriteD(item.ObjId);
                WriteD(item.Template.ItemId);
                WriteD(0);
                WriteQ(item.Count);
                WriteH(item.Template.Type2);
                WriteH(item.CustomType1);
                WriteH(0);
                WriteD(item.Template.BodyPart);
                WriteH(item.Enchant);
                WriteH(item.CustomType2);
                WriteD(item.AugmentationId);
                WriteD(item.Durability);
                WriteD(item.LifeTimeEnd());

                WriteH(item.AttrAttackType);
                WriteH(item.AttrAttackValue);
                WriteH(item.AttrDefenseValueFire);
                WriteH(item.AttrDefenseValueWater);
                WriteH(item.AttrDefenseValueWind);
                WriteH(item.AttrDefenseValueEarth);
                WriteH(item.AttrDefenseValueHoly);
                WriteH(item.AttrDefenseValueUnholy);

                WriteH(item.Enchant1);
                WriteH(item.Enchant2);
                WriteH(item.Enchant3);

                WriteQ(item.Template.ReferencePrice / 2);
            }

            WriteH(_refund.Count);

            int idx = 0;
            foreach (L2Item item in _refund)
            {
                WriteD(item.ObjId);
                WriteD(item.Template.ItemId);
                WriteD(0);
                WriteQ(item.Count);
                WriteH(item.Template.Type2);
                WriteH(item.CustomType1);
                WriteH(0);
                WriteD(item.Template.BodyPart);
                WriteH(item.Enchant);
                WriteH(item.CustomType2);
                WriteD(item.AugmentationId);
                WriteD(item.Durability);
                WriteD(item.LifeTimeEnd());

                WriteH(item.AttrAttackType);
                WriteH(item.AttrAttackValue);
                WriteH(item.AttrDefenseValueFire);
                WriteH(item.AttrDefenseValueWater);
                WriteH(item.AttrDefenseValueWind);
                WriteH(item.AttrDefenseValueEarth);
                WriteH(item.AttrDefenseValueHoly);
                WriteH(item.AttrDefenseValueUnholy);

                WriteH(item.Enchant1);
                WriteH(item.Enchant2);
                WriteH(item.Enchant3);

                WriteD(idx++);
                WriteQ(item.Template.ReferencePrice / 2 * item.Count);
            }

            WriteC(0);
        }
    }
}