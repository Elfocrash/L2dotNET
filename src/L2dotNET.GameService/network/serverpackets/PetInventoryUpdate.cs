using System.Collections.Generic;
using L2dotNET.GameService.Model.Items;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class PetInventoryUpdate : GameServerNetworkPacket
    {
        protected List<object[]> Update = new List<object[]>();

        public void AddNewItem(L2Item item)
        {
            Update.Add(new object[] { item, (short)1 });
        }

        public void AddModItem(L2Item item)
        {
            Update.Add(new object[] { item, (short)2 });
        }

        public void AddDelItem(L2Item item)
        {
            Update.Add(new object[] { item, (short)3 });
        }

        protected internal override void Write()
        {
            WriteC(0xb4);
            WriteH(Update.Count);

            foreach (object[] obj in Update)
            {
                WriteH((short)obj[1]);

                L2Item item = (L2Item)obj[0];

                WriteD(item.ObjId);
                WriteD(item.Template.ItemId);
                WriteD(0); //loc
                WriteQ(item.Count);

                WriteH(item.Template.Type2);
                WriteH(0);
                WriteH(item.IsEquipped);

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