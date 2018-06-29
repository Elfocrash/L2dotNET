using System.Collections.Generic;
using L2dotNET.DataContracts;

namespace L2dotNET.Services.Contracts
{
    public interface IItemService
    {
        List<ItemContract> RestoreInventory(int objId, string location);
    }
}