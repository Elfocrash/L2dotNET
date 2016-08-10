using L2dotNET.model.items;
using L2dotNET.model.player;
using L2dotNET.world;

namespace L2dotNET.model.inventory
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