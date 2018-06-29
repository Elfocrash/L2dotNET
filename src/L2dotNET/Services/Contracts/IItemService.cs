using System.Collections.Generic;
using System.Threading.Tasks;
using L2dotNET.DataContracts;

namespace L2dotNET.Services.Contracts
{
    public interface IItemService
    {
        Task<IEnumerable<ItemContract>> RestoreInventory(int characterId);
    }
}