using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<ItemContract> RestoreInventory(int objId, string location)
        {
            return _itemRepository.RestoreInventory(objId, location);
        }
    }
}