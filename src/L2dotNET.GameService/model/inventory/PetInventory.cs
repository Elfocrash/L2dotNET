using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Inventory
{
    public class PetInventory : Inventory
    {
        public PetInventory(L2Character owner)
        {
            Owner = owner;
        }

        protected override L2Character Owner { get; }
        protected override L2Item.ItemLocation BaseLocation { get; }
    }
}
