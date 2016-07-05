using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Inventory
{
    public class PetInventory : Inventory
    {
        public PetInventory(L2Character owner) : base(owner)
        {
            Owner = owner;
        }

        protected override L2Character Owner { get; }
        protected override L2Item.ItemLocation BaseLocation { get; }
    }
}