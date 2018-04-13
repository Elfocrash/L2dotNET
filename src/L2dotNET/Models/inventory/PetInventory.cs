using L2dotNET.Models.Items;

namespace L2dotNET.Models.Inventory
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