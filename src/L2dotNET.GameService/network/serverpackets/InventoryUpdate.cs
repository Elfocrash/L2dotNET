using System.Collections.Generic;
using L2dotNET.GameService.Model.Items;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class InventoryUpdate : GameServerNetworkPacket
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
            WriteC(0x27);
            WriteH(Update.Count);

            foreach (object[] obj in Update)
            {
                WriteH((short)obj[1]);

                L2Item item = (L2Item)obj[0];

                WriteD(item.ObjId);
                WriteD(item.Template.ItemId);
                WriteD(item.SlotLocation); //loc
                WriteQ(item.Count);

                WriteH(item.Template.Type2());
                WriteH(item.CustomType1);
                WriteH(item.IsEquipped);

                WriteD(item.Template.BodyPartId());
                WriteH(item.Enchant);
                WriteH(item.CustomType2);

                WriteD(item.AugmentationId);
                WriteD(item.Durability);
                //writeD(item.LifeTimeEnd());
            }
        }
    }
}