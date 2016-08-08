using System.Collections.Generic;
using L2dotNET.Models;
using L2dotNET.Repositories;
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
            Dictionary<int, ArmorModel> armors = new Dictionary<int, ArmorModel>();
            List<ArmorModel> armorModels = _unitOfWork.ItemRepository.GetAllArmors();

            foreach (ArmorModel model in armorModels)
            {
                armors.Add(model.ItemId, model);
            }

            return armors;
        }

        public Dictionary<int, WeaponModel> GetAllWeaponModelsDict()
        {
            Dictionary<int, WeaponModel> weapons = new Dictionary<int, WeaponModel>();
            List<WeaponModel> weaponModels = _unitOfWork.ItemRepository.GetAllWeapons();

            foreach (WeaponModel model in weaponModels)
            {
                weapons.Add(model.ItemId, model);
            }

            return weapons;
        }

        public Dictionary<int, EtcItemModel> GetAllEtcItemModelsDict()
        {
            Dictionary<int, EtcItemModel> etcItems = new Dictionary<int, EtcItemModel>();
            List<EtcItemModel> etcItemModels = _unitOfWork.ItemRepository.GetAllEtcItems();

            foreach (EtcItemModel model in etcItemModels)
            {
                etcItems.Add(model.ItemId, model);
            }

            return etcItems;
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