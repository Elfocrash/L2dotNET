using L2dotNET.DataContracts;
using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Services.Contracts;
using L2dotNET.Tables;

namespace L2dotNET.Models.Inventory
{
    public class PetInventory : Inventory
    {
        public PetInventory(L2Character owner)
            : base(owner)
        {
        }
    }
}