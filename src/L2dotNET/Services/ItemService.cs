using System.Collections.Generic;
using System.Linq;
using L2dotNET.DataContracts;
using L2dotNET.Repositories.Contracts;
using L2dotNET.Services.Contracts;

namespace L2dotNET.Services
{
    public class ItemService : IItemService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<ArmorContract> GetAllArmorsList()
        {
            return _unitOfWork.ItemRepository.GetAllArmors();
        }

        public Dictionary<int, ArmorContract> GetAllArmorModelsDict()
        {
            List<ArmorContract> armorModels = _unitOfWork.ItemRepository.GetAllArmors();

            return armorModels.ToDictionary(model => model.ItemId);
        }

        public Dictionary<int, WeaponContract> GetAllWeaponModelsDict()
        {
            List<WeaponContract> weaponModels = _unitOfWork.ItemRepository.GetAllWeapons();

            return weaponModels.ToDictionary(model => model.ItemId);
        }

        public Dictionary<int, EtcItemContract> GetAllEtcItemModelsDict()
        {
            List<EtcItemContract> etcItemModels = _unitOfWork.ItemRepository.GetAllEtcItems();

            return etcItemModels.ToDictionary(model => model.ItemId);
        }

        public void InsertNewItem(ItemContract item)
        {
            _unitOfWork.ItemRepository.InsertNewItem(item);
        }

        public void UpdateItem(ItemContract item)
        {
            _unitOfWork.ItemRepository.UpdateItem(item);
        }

        public List<ItemContract> RestoreInventory(int objId, string location)
        {
            return _unitOfWork.ItemRepository.RestoreInventory(objId, location);
        }

        public List<WeaponContract> GetAllWeapons()
        {
            return _unitOfWork.ItemRepository.GetAllWeapons();
        }

        public List<EtcItemContract> GetAllEtcItems()
        {
            return _unitOfWork.ItemRepository.GetAllEtcItems();
        }
    }
}