using L2dotNET.model.items;
using L2dotNET.world;

namespace L2dotNET.model.inventory
{
    public class PetInventory : Inventory
    {
        public PetInventory(L2Character owner) : base(owner)
        {
            Owner = owner;
        }

        protected override L2Character Owner { get; set; }
        protected override L2Item.ItemLocation BaseLocation { get; }
    }
}