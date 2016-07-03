using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Inventory
{
    public class PcInventory : Inventory
    {
        public PcInventory(L2Character owner)
        {
            Owner = owner;
        }

        protected override L2Character Owner { get; }
        protected override L2Item.ItemLocation BaseLocation { get; }

        public bool ReduceAdena(int count, L2Player player)
        {
            return false;
        }

    }
}
