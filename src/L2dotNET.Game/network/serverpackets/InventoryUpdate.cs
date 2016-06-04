using System.Collections.Generic;
using L2dotNET.GameService.model.items;

namespace L2dotNET.GameService.network.l2send
{
    class InventoryUpdate : GameServerNetworkPacket
    {
        protected List<object[]> _update = new List<object[]>();
        public void addNewItem(L2Item item)
        {
            _update.Add(new object[] { item, (short)1 });
        }

        public void addModItem(L2Item item)
        {
            _update.Add(new object[] { item, (short)2 });
        }

        public void addDelItem(L2Item item)
        {
            _update.Add(new object[] { item, (short)3 });
        }

        protected internal override void write()
        {
            writeC(0x27);
            writeH(_update.Count);

            foreach (object[] obj in _update) 
            {
                writeH((short)obj[1]); 

                L2Item item = (L2Item)obj[0];

                writeD(item.ObjID);
                writeD(item.Template.ItemID);
                writeD(item.SlotLocation); //loc
                writeQ(item.Count);

                writeH(item.Template.Type2());
                writeH(item.CustomType1);
                writeH(item._isEquipped);

                writeD(item.Template.BodyPartId());
                writeH(item.Enchant);
                writeH(item.CustomType2);

                writeD(item.AugmentationID);
                writeD(item.Durability);
                //writeD(item.LifeTimeEnd());

		    }
        }
    }
}
