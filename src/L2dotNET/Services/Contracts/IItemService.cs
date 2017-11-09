using System.Collections.Generic;
using L2dotNET.DataContracts;

namespace L2dotNET.Services.Contracts
{
    public interface IItemService
    {
        List<ArmorContract> GetAllArmorsList();

        Dictionary<int, ArmorContract> GetAllArmorModelsDict();

        void InsertNewItem(ItemContract item);

        void UpdateItem(ItemContract item);

        List<ItemContract> RestoreInventory(int objId, string location);

        List<WeaponContract> GetAllWeapons();

        Dictionary<int, WeaponContract> GetAllWeaponModelsDict();

        List<EtcItemContract> GetAllEtcItems();

        Dictionary<int, EtcItemContract> GetAllEtcItemModelsDict();
    }
}