using System.Collections.Generic;
using System.Linq;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ItemList : GameServerNetworkPacket
    {
        private readonly bool showWindow;
        private readonly List<ItemListItem> items = new List<ItemListItem>();
        private readonly List<int> blocked = new List<int>();

        public ItemList(L2Player player, bool showWindow)
        {
            this.showWindow = showWindow;
            foreach (L2Item item in player.Inventory.Items.Values.Where(item => item.Template.Type != ItemTemplate.L2ItemType.questitem))
            {
                items.Add(new ItemListItem { ObjectId = item.ObjID, ItemId = item.Template.ItemID, Slot = item.SlotLocation, Count = item.Count, Type2 = item.Template.Type2(), CType1 = item.CustomType1, Equip = item._isEquipped, Bodypart = item.Template.BodyPartId(), Enchant = item.Enchant, CType2 = item.CustomType2, Augment = item.AugmentationID, Mana = item.Durability, TimeLeft = item.LifeTimeEnd() });

                if (item.Blocked)
                    blocked.Add(item.ObjID);
            }
        }

        protected internal override void write()
        {
            writeC(0x1b);
            writeH(showWindow ? 1 : 0);
            writeH(items.Count);

            foreach (ItemListItem item in items)
            {
                writeD(item.ObjectId);
                writeD(item.ItemId);
                writeD(item.Slot);
                writeQ(item.Count);
                writeH(item.Type2);
                writeH(item.CType1);
                writeH(item.Equip);
                writeD(item.Bodypart);
                writeH(item.Enchant);
                writeH(item.CType2);
                writeD(item.Augment);
                writeD(item.Mana);
                //writeD(item.TimeLeft);
            }

            //writeH(blocked.Count);
            //if (blocked.Count > 0)
            //{
            //    writeC(2);
            //    foreach (int id in blocked)
            //        writeD(id);
            //}

            //items = null;
            //blocked = null;
        }
    }
}