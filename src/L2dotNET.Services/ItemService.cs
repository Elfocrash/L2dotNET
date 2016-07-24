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
    }
}