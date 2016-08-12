using System.Collections.Generic;
using System.Linq;
using L2dotNET.Models;
using L2dotNET.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace L2dotNET.Tests
{
    [TestClass]
    public class ItemRetrieval
    {
        [TestMethod]
        public void GetArmors()
        {
            List<ArmorModel> ArmorModels = new ItemRepository().GetAllArmors();

            Dictionary<int, ArmorModel> Armors = ArmorModels.ToDictionary(model => model.ItemId);
        }
    }
}
