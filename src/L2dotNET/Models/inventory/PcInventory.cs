using L2dotNET.Models.items;
using L2dotNET.Models.player;

namespace L2dotNET.Models.inventory
{
    public class PcInventory : Inventory
    {
        public PcInventory(L2Character owner) : base(owner)
        {
            Owner = owner;
        }

        protected override L2Character Owner { get; set; }
        protected override L2Item.ItemLocation BaseLocation { get; }

        public bool ReduceAdena(int count, L2Player player)
        {
            return false;
        }

    }
}