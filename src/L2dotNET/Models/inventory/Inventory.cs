using System.Collections.Generic;
using System.Linq;
using L2dotNET.DataContracts;
using L2dotNET.model.items;
using L2dotNET.model.player;
using L2dotNET.world;

namespace L2dotNET.model.inventory
{
    public class Inventory : ItemContainer
    {
        public Inventory(L2Character owner)
        {
            Owner = owner;
            Paperdoll = new L2Item[PaperdollTotalslots];
        }

        protected override L2Character Owner { get; set; }
        protected override L2Item.ItemLocation BaseLocation { get; }

        public static readonly int PaperdollUnder = 0;
        public static readonly int PaperdollLear = 1;
        public static readonly int PaperdollRear = 2;
        public static readonly int PaperdollNeck = 3;
        public static readonly int PaperdollLfinger = 4;
        public static readonly int PaperdollRfinger = 5;
        public static readonly int PaperdollHead = 6;
        public static readonly int PaperdollRhand = 7;
        public static readonly int PaperdollLhand = 8;
        public static readonly int PaperdollGloves = 9;
        public static readonly int PaperdollChest = 10;
        public static readonly int PaperdollLegs = 11;
        public static readonly int PaperdollFeet = 12;
        public static readonly int PaperdollBack = 13;
        public static readonly int PaperdollFace = 14;
        public static readonly int PaperdollHair = 15;
        public static readonly int PaperdollHairall = 16;
        public static readonly int PaperdollTotalslots = 17;

        public L2Item[] Paperdoll;

        public L2Item GetPaperdollItem(int slot)
        {
            return Paperdoll[slot];
        }

        public List<L2Item> GetPaperdollItems()
        {
            return Paperdoll.Where(item => item != null).ToList();
        }

        public override void Restore(L2Character owner)
        {
            List<ItemContract> models = ItemService.RestoreInventory(owner.ObjId, "Inventory");
            List<L2Item> items = L2Item.RestoreFromDb(models);

            foreach (L2Item item in items)
            {
                L2World.Instance.AddObject(item);
                Owner = owner;
                AddItem(item, (L2Player)Owner);
            }
        }

        public static int GetPaperdollIndex(int slot)
        {
            switch (slot)
            {
                case 0x0001:
                    return PaperdollUnder;
                case 0x0002:
                    return PaperdollRear;
                case 0x0004:
                    return PaperdollLear;
                case 0x0008:
                    return PaperdollNeck;
                case 0x0010:
                    return PaperdollRfinger;
                case 0x0020:
                    return PaperdollLfinger;
                case 0x0040:
                    return PaperdollHead;
                case 0x0080:
                case 0x4000:
                    return PaperdollRhand;
                case 0x0100:
                    return PaperdollLhand;
                case 0x0200:
                    return PaperdollGloves;
                case 0x0400:
                case 0x8000:
                case 0x020000:
                    return PaperdollChest;
                case 0x0800:
                    return PaperdollLegs;
                case 0x1000:
                    return PaperdollFeet;
                case 0x2000:
                    return PaperdollBack;
                case 0x010000:
                case 0x080000:
                    return PaperdollFace;
                case 0x040000:
                    return PaperdollHair;
            }

            return -1;
        }
    }
}