using L2dotNET.Models.Items;
using L2dotNET.Services.Contracts;
using L2dotNET.Tables;

namespace L2dotNET.Models.Inventory
{
    public class PetInventory : Inventory
    {
        public PetInventory(IItemService itemService, IdFactory idFactory, ItemTable itemTable, L2Character owner) : base(itemService, idFactory,itemTable,owner)
        {
            Owner = owner;
        }

        protected override L2Character Owner { get; set; }
        protected override L2Item.ItemLocation BaseLocation { get; }
    }
}