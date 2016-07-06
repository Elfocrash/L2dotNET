using System.Collections.Generic;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ItemList : GameServerNetworkPacket
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
                {
                    _blocked.Add(item.ObjId);
                }
            }
        }

        protected internal override void Write()
        {
            WriteC(0x1b);
            WriteH(_showWindow ? 1 : 0);
            WriteH(_items.Count);

            foreach (ItemListItem item in _items)
            {
                WriteD(item.ObjectId);
                WriteD(item.ItemId);
                WriteD(item.Slot);
                WriteQ(item.Count);
                WriteH(item.Type2);
                WriteH(item.CType1);
                WriteH(item.Equip);
                WriteD(item.Bodypart);
                WriteH(item.Enchant);
                WriteH(item.CType2);
                WriteD(item.Augment);
                WriteD(item.Mana);
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