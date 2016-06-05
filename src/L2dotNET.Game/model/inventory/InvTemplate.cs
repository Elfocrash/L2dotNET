using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using L2dotNET.GameService.model.items;

namespace L2dotNET.GameService.model.inventory
{
    [Synchronization]
    public class InvTemplate
    {
        public Dictionary<int, L2Item> Items = new Dictionary<int, L2Item>();

        public virtual void addItem(ItemTemplate template, long count, short enchant, bool msg, bool update) { }
    }
}