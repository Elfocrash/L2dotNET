using System.Collections.Generic;
using L2dotNET.Models.Items;

namespace L2dotNET.Network.serverpackets
{
    class InventoryUpdate : GameserverPacket
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

        public override void Write()
        {
            WriteByte(0x27);
            WriteShort(Update.Count);

            foreach (object[] obj in Update)
            {
                WriteShort((short)obj[1]);

                L2Item item = (L2Item)obj[0];

                WriteInt(item.CharacterId);
                WriteInt(item.Template.ItemId);
                WriteInt(item.SlotLocation); //loc
                WriteLong(item.Count);

                WriteShort(item.Template.Type2);
                WriteShort(item.CustomType1);
                WriteShort(item.IsEquipped);

                WriteInt((int) item.Template.BodyPart);
                WriteShort(item.Enchant);
                WriteShort(item.CustomType2);

                WriteInt(item.AugmentationId);
                WriteInt(item.Durability);
                //writeD(item.LifeTimeEnd());
            }
        }
    }
}