using System.Collections.Generic;
using L2dotNET.Models;

namespace L2dotNET.Services.Contracts
{
    public interface IItemService
    {
        List<ArmorModel> GetAllArmorsList();
        Dictionary<int, ArmorModel> GetAllArmorModelsDict();
    }
}