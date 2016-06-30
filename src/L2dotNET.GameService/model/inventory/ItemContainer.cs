using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.GameService.Model.Inventory
{
    public abstract class ItemContainer
    {
        protected List<L2Item> items;

        protected ItemContainer()
        {
            items = new List<L2Item>();
        }

        protected abstract L2Character Owner { get; }

        public int OwnerId { get { return Owner != null ? Owner.ObjID : 0; } }

        public int Count { get { return items.Count; } }
    }
}
