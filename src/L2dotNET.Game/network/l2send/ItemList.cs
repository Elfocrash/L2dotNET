using System.Collections.Generic;
using L2dotNET.Game.model.items;

namespace L2dotNET.Game.network.l2send
{
    class ItemList : GameServerNetworkPacket
    {
        private bool showWindow;
        private List<ItemListItem> items = new List<ItemListItem>();
        private List<int> blocked = new List<int>();
        public ItemList(L2Player player, bool showWindow)
        {
            this.showWindow = showWindow;
            foreach (L2Item item in player.Inventory.Items.Values)
            {
                if (item.Template.Type == ItemTemplate.L2ItemType.questitem)
                    continue;

                items.Add(new ItemListItem
                {
                    ObjectId = item.ObjID,
                    ItemId = item.Template.ItemID,
                    Slot = item.SlotLocation,
                    Count = item.Count,
                    Type2 = item.Template.Type2(),
                    CType1 = item.CustomType1,
                    Equip = item._isEquipped,
                    Bodypart = item.Template.BodyPartId(),
                    Enchant = item.Enchant,
                    CType2 = item.CustomType2,
                    Augment = item.AugmentationID,
                    Mana = item.Durability,
                    TimeLeft = item.LifeTimeEnd(),
                    AtAttackType = item.AttrAttackType,
                    AtAttackVal = item.AttrAttackValue,
                    AtDefenseValFire = item.AttrDefenseValueFire,
                    AtDefenseValWater = item.AttrDefenseValueWater,
                    AtDefenseValWind = item.AttrDefenseValueWind,
                    AtDefenseValEarth = item.AttrDefenseValueEarth,
                    AtDefenseValHoly = item.AttrDefenseValueHoly,
                    AtDefenseValUnholy = item.AttrDefenseValueUnholy,
                    E1 = item.Enchant1,
                    E2 = item.Enchant2,
                    E3 = item.Enchant3
                });

                if (item.Blocked)
                    blocked.Add(item.ObjID);
            }
        }

        protected internal override void write()
        {
            writeC(0x11);
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
                writeD(item.TimeLeft);
                writeH(item.AtAttackType);
                writeH(item.AtAttackVal);
                writeH(item.AtDefenseValFire);
                writeH(item.AtDefenseValWater);
                writeH(item.AtDefenseValWind);
                writeH(item.AtDefenseValEarth);
                writeH(item.AtDefenseValHoly);
                writeH(item.AtDefenseValUnholy);
                writeH(item.E1);
                writeH(item.E2);
                writeH(item.E3);
            }

            writeH(blocked.Count);
            if (blocked.Count > 0)
            {
                writeC(2);
                foreach (int id in blocked)
                    writeD(id);
            }

            items = null;
            blocked = null;
        }
    }

    class ItemListItem
    {
        public int ObjectId;
        public int ItemId;
        public int Slot;
        public long Count;
        public short Type2;
        public int CType1;
        public short Equip;
        public int Bodypart;
        public int Enchant;
        public int CType2;
        public int Augment;
        public int Mana;
        public int TimeLeft;
        public short AtAttackType;
        public short AtAttackVal;
        public short AtDefenseValFire;
        public short AtDefenseValWater;
        public short AtDefenseValWind;
        public short AtDefenseValEarth;
        public short AtDefenseValHoly;
        public short AtDefenseValUnholy;
        public short E1;
        public short E2;
        public short E3;
    }
}
