using System.Collections.Generic;
using L2dotNET.Models;

namespace L2dotNET.Repositories.Contracts
{
    public interface IItemRepository
    {
        List<ArmorModel> GetAllArmors();

        void InsertNewItem(ItemModel item);

        void UpdateItem(ItemModel item);

        List<ItemModel> RestoreInventory(int objId, string location);

        List<WeaponModel> GetAllWeapons();

        List<EtcItemModel> GetAllEtcItems();
    }
}