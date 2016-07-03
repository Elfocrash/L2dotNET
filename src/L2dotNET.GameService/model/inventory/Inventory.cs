using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Inventory
{
    public class Inventory : ItemContainer
    {
        public Inventory()
        {
            Paperdoll = (L2Item[]) new ArrayList().ToArray(typeof(L2Item));
        }

        protected override L2Character Owner { get; }
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
            List<L2Item> itemList = new List<L2Item>();

            foreach (L2Item item in Paperdoll)
            {
                if(item != null)
                    itemList.Add(item);
            }
            return itemList;
        }
    }
}
