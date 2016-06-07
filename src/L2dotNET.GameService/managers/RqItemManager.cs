using System.Collections.Generic;
using L2dotNET.GameService.Model.Items;

namespace L2dotNET.GameService.Managers
{
    class RqItemManager
    {
        private static readonly RqItemManager m = new RqItemManager();

        public static RqItemManager getInstance()
        {
            return m;
        }

        public SortedList<int, L2Item> _items = new SortedList<int, L2Item>();

        public void postItem(L2Item item)
        {
            if (_items.ContainsKey(item.ObjID))
                lock (_items)
                    _items.Remove(item.ObjID);

            _items.Add(item.ObjID, item);
        }

        public L2Item getItem(int _objectId)
        {
            return _items.ContainsKey(_objectId) ? _items[_objectId] : null;
        }
    }
}