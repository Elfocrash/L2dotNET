using L2dotNET.Models.items;

namespace L2dotNET.Models.inventory
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