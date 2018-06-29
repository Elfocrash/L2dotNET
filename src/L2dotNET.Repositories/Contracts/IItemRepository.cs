using System.Collections.Generic;
using L2dotNET.DataContracts;

namespace L2dotNET.Repositories.Contracts
{
    public interface IItemRepository
    {
        List<ItemContract> RestoreInventory(int objId, string location);
    }
}