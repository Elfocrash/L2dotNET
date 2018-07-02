using System.Collections.Generic;
using System.Threading.Tasks;
using L2dotNET.DataContracts;

namespace L2dotNET.Repositories.Contracts
{
    public interface IItemRepository
    {
        Task<IEnumerable<ItemContract>> RestoreInventory(int characterId);
        int GetMaxItemId();
    }
}