using System.Collections.Generic;
using System.Threading.Tasks;
using L2dotNET.DataContracts;
using L2dotNET.Repositories.Contracts;
using L2dotNET.Services.Contracts;

namespace L2dotNET.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<IEnumerable<ItemContract>> RestoreInventory(int characterId)
        {
            return await _itemRepository.RestoreInventory(characterId);
        }
    }
}