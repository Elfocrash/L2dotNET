using System.Collections.Generic;
using L2dotNET.GameService.Model.Items;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExPostItemList : GameServerNetworkPacket
    {
        private readonly List<L2Item> list;

        public ExPostItemList(List<L2Item> list)
        {
            this.list = list;
        }

        protected internal override void write()
        {
            writeC(0xfe);
            writeH(0xb2);
            writeD(list.Count);

            foreach (L2Item item in list)
            {
                writeD(item.ObjID);
                writeD(item.Template.ItemID);
                writeD(0);
                writeQ(item.Count);

                writeH(item.Template.Type2());
                writeH(0);
                writeH(0);

                writeD(item.Template.BodyPartId());
                writeH(item.Enchant);
                writeH(0);

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
            }
        }
    }
}