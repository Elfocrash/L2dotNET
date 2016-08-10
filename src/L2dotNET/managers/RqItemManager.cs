using System.Collections.Generic;
using L2dotNET.model.items;

namespace L2dotNET.managers
{
    class RqItemManager
    {
        private static readonly RqItemManager M = new RqItemManager();

        public static RqItemManager GetInstance()
        {
            return M;
        }

        public SortedList<int, L2Item> Items = new SortedList<int, L2Item>();

        public void PostItem(L2Item item)
        {
            if (Items.ContainsKey(item.ObjId))
            {
                lock (Items)
                    Items.Remove(item.ObjId);
            }

            Items.Add(item.ObjId, item);
        }

        public L2Item GetItem(int objectId)
        {
            return Items.ContainsKey(objectId) ? Items[objectId] : null;
        }
    }
}