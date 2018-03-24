using System.Collections.Generic;
using L2dotNET.Models.items;
using L2dotNET.Models.player;

namespace L2dotNET.Network.serverpackets
{
    class ItemList : GameserverPacket
    {
        private readonly bool _showWindow;
        private readonly List<ItemListItem> _items = new List<ItemListItem>();
        private readonly List<int> _blocked = new List<int>();

        public ItemList(L2Player player, bool showWindow)
        {
            _showWindow = showWindow;
            foreach (L2Item item in player.Inventory.Items)
            {
                _items.Add(new ItemListItem
                {
                    ObjectId = item.ObjId,
                    ItemId = item.Template.ItemId,
                    Slot = item.SlotLocation,
                    Count = item.Count,
                    Type2 = (short)item.Template.Type2,
                    CType1 = item.CustomType1,
                    Equip = item.IsEquipped,
                    Bodypart = item.Template.BodyPart,
                    Enchant = item.Enchant,
                    CType2 = item.CustomType2,
                    Augment = item.AugmentationId,
                    Mana = item.Durability,
                    TimeLeft = item.LifeTimeEnd()
                });

                if (item.Blocked)
                    _blocked.Add(item.ObjId);
            }
        }

        public override void Write()
        {
            WriteByte(0x1b);
            WriteShort(_showWindow ? 1 : 0);
            WriteShort(_items.Count);

            foreach (ItemListItem item in _items)
            {


                WriteShort(item.CType1);
                WriteInt(item.ObjectId);
                WriteInt(item.ItemId);
                WriteInt(item.Count);
                WriteShort(item.Type2);
                WriteShort(item.CType1);

                WriteShort(item.Equip);
                WriteInt(item.Bodypart);
                WriteShort(item.Enchant);
                WriteShort(item.CType2);
                WriteInt(item.Augment);
                WriteInt(item.Mana);
            }
        }
    }
}