using System.Collections.Generic;
using L2dotNET.Models;

namespace L2dotNET.Repositories.Contracts
{
    public interface IItemRepository
    {
        List<ArmorModel> GetAllArmors();
    }
}