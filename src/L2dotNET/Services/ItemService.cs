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

        public List<ArmorContract> GetAllArmorsList()
        {
            return _itemRepository.GetAllArmors();
        }

        public Dictionary<int, ArmorContract> GetAllArmorModelsDict()
        {
            List<ArmorContract> armorModels = _itemRepository.GetAllArmors();

            return armorModels.ToDictionary(model => model.ArmorId);
        }

        public Dictionary<int, WeaponContract> GetAllWeaponModelsDict()
        {
            List<WeaponContract> weaponModels = _itemRepository.GetAllWeapons();

            return weaponModels.ToDictionary(model => model.ItemId);
        }

        public Dictionary<int, EtcItemContract> GetAllEtcItemModelsDict()
        {
            List<EtcItemContract> etcItemModels = _itemRepository.GetAllEtcItems();

            return etcItemModels.ToDictionary(model => model.ItemId);
        }

        public void InsertNewItem(ItemContract item)
        {
            _itemRepository.InsertNewItem(item);
        }

        public void UpdateItem(ItemContract item)
        {
            _itemRepository.UpdateItem(item);
        }

        public List<ItemContract> RestoreInventory(int objId, string location)
        {
            return _itemRepository.RestoreInventory(objId, location);
        }

        public List<WeaponContract> GetAllWeapons()
        {
            return _itemRepository.GetAllWeapons();
        }

        public List<EtcItemContract> GetAllEtcItems()
        {
            return _itemRepository.GetAllEtcItems();
        }
    }
}