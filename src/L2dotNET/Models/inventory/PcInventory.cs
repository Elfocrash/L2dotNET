using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Models.Items;
using L2dotNET.Models.Player;
using L2dotNET.Services.Contracts;
using L2dotNET.Tables;

namespace L2dotNET.Models.Inventory
{
    public class PcInventory : Inventory
    {
        public PcInventory(IItemService itemService, IdFactory idFactory, ItemTable itemTable, L2Character owner) : base(itemService, idFactory, itemTable, owner)
        {
            Owner = owner;
        }

        protected override L2Character Owner { get; set; }
        protected override ItemLocation BaseLocation { get; }

        public bool ReduceAdena(int count, L2Player player)
        {
            return false;
        }

    }
}