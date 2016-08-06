using System.Collections.Generic;
using L2dotNET.Models;

namespace L2dotNET.Services.Contracts
{
    public interface IItemService
    {
        List<ArmorModel> GetAllArmorsList();

        Dictionary<int, ArmorModel> GetAllArmorModelsDict();

        void InsertNewItem(ItemModel item);

        void UpdateItem(ItemModel item);

        List<ItemModel> RestoreInventory(int objId, string location);

        List<WeaponModel> GetAllWeapons();

        Dictionary<int, WeaponModel> GetAllWeaponModelsDict();
    }
}