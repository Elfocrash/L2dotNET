using System.Collections.Generic;
using L2dotNET.GameService.Model.Items;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExPostItemList : GameServerNetworkPacket
    {
        private readonly List<L2Item> _list;

        public ExPostItemList(List<L2Item> list)
        {
            this._list = list;
        }

        protected internal override void Write()
        {
            WriteC(0xfe);
            WriteH(0xb2);
            WriteD(_list.Count);

            foreach (L2Item item in _list)
            {
                WriteD(item.ObjId);
                WriteD(item.Template.ItemId);
                WriteD(0);
                WriteQ(item.Count);

                WriteH(item.Template.Type2);
                WriteH(0);
                WriteH(0);

                WriteD(item.Template.BodyPart);
                WriteH(item.Enchant);
                WriteH(0);

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
            }
        }
    }
}