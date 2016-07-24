using System;
using System.Collections.Generic;
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
            List<ArmorModel> ArmorModels = new List<ArmorModel>();
            Dictionary<int, ArmorModel> Armors = new Dictionary<int, ArmorModel>();
            ArmorModels = new ItemRepository().GetAllArmors();

            foreach (ArmorModel model in ArmorModels)
            {
                Armors.Add(model.ItemId, model);
            }
        }
    }
}
