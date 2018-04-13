using L2dotNET.Models.Items;
using L2dotNET.Models.Player;

namespace L2dotNET.Models.Inventory
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