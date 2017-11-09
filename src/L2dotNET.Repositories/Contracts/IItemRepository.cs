using System.Collections.Generic;
using L2dotNET.DataContracts;

namespace L2dotNET.Repositories.Contracts
{
    public interface IItemRepository
    {
        List<ArmorContract> GetAllArmors();

        void InsertNewItem(ItemContract item);

        void UpdateItem(ItemContract item);

        List<ItemContract> RestoreInventory(int objId, string location);

        List<WeaponContract> GetAllWeapons();

        List<EtcItemContract> GetAllEtcItems();
    }
}