using L2dotNET.DataContracts;
using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Services.Contracts;
using L2dotNET.Tables;

namespace L2dotNET.Models.Inventory
{
    public class PetInventory : Inventory
    {
        public PetInventory(ICrudService<ItemContract> itemCrudService,
            IItemService itemService,
            IdFactory idFactory,
            ItemTable itemTable,
            L2Character owner)
            : base(itemCrudService, itemService, idFactory, itemTable, owner)
        {
            Owner = owner;
        }

        protected override L2Character Owner { get; set; }
        protected override ItemLocation BaseLocation { get; }
    }
}