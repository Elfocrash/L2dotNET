using System.Collections.Generic;
using System.Linq;
using L2dotNET.Models;
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

        public List<ArmorModel> GetAllArmorsList()
        {
            return _unitOfWork.ItemRepository.GetAllArmors();
        }

        public Dictionary<int, ArmorModel> GetAllArmorModelsDict()
        {
            List<ArmorModel> armorModels = _unitOfWork.ItemRepository.GetAllArmors();

            return armorModels.ToDictionary(model => model.ItemId);
        }

        public Dictionary<int, WeaponModel> GetAllWeaponModelsDict()
        {
            List<WeaponModel> weaponModels = _unitOfWork.ItemRepository.GetAllWeapons();

            return weaponModels.ToDictionary(model => model.ItemId);
        }

        public Dictionary<int, EtcItemModel> GetAllEtcItemModelsDict()
        {
            List<EtcItemModel> etcItemModels = _unitOfWork.ItemRepository.GetAllEtcItems();

            return etcItemModels.ToDictionary(model => model.ItemId);
        }

        public void InsertNewItem(ItemModel item)
        {
            _unitOfWork.ItemRepository.InsertNewItem(item);
        }

        public void UpdateItem(ItemModel item)
        {
            _unitOfWork.ItemRepository.UpdateItem(item);
        }

        public List<ItemModel> RestoreInventory(int objId, string location)
        {
            return _unitOfWork.ItemRepository.RestoreInventory(objId, location);
        }

        public List<WeaponModel> GetAllWeapons()
        {
            return _unitOfWork.ItemRepository.GetAllWeapons();
        }

        public List<EtcItemModel> GetAllEtcItems()
        {
            return _unitOfWork.ItemRepository.GetAllEtcItems();
        }
    }
}