using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Inventory
{
    [Synchronization]
    public abstract class ItemContainer
    {
        public List<L2Item> Items;

        protected ItemContainer()
        {
            Items = new List<L2Item>();
        }

        protected abstract L2Character Owner { get; }

        protected abstract L2Item.ItemLocation BaseLocation { get; }

        public int OwnerId => Owner?.ObjId ?? 0;

        public int Count => Items.Count;

        public string Name => "ItemContainer";

        public bool HasAtLeaseOneItem(params int[] itemIds)
        {
            foreach (int itemId in itemIds)
            {
                if (GetItemsByItemId(itemId) != null)
                {
                    return true;
                }
            }

            return false;
        }

        public List<L2Item> GetItemsByItemId(int itemId)
        {
            List<L2Item> list = new List<L2Item>();
            foreach (L2Item item in Items)
            {
                if (item.Template.ItemId == itemId)
                {
                    list.Add(item);
                }
            }

            return list;
        }

        public L2Item GetItemByItemId(int itemId)
        {
            foreach (L2Item item in Items)
            {
                if (item.Template.ItemId == itemId)
                {
                    return item;
                }
            }

            return null;
        }

        public L2Item GetItemByObjectId(int objectId)
        {
            foreach (L2Item item in Items)
            {
                if (item.ObjId == objectId)
                {
                    return item;
                }
            }

            return null;
        }

        public L2Item AddItem(L2Item item, L2Player player)
        {
            return null;
            //to be implemented
        }

        public L2Item AddItem(int itemId, int count, L2Player player)
        {
            if(count <= 0)
                return null;

            

            return null;
        }

        public L2Item DestroyItem(L2Item item, int count, L2Player player)
        {
            return null;
            //to be implemented
        }

        public L2Item DestroyItemById(int itemId, int count, L2Player player)
        {
            return null;
            //to be implemented
        }

        public void DestroyAllItems(L2Player player)
        {
            foreach (L2Item item in Items)
            {
                DestroyItem(item, item.Count, player);
            }
        }

        public int AdenaCount()
        {
            foreach (L2Item item in Items)
                if (item.Template.ItemId == 57)
                {
                    return item.Count;
                }

            return 0;
        }

        public L2Item AddItem(L2Item item)
        {
            return null;
            //to be implemented
        }

        protected bool RemoveItem(L2Item item)
        {
            return Items.Remove(item);
        }

        public void RestoreInv() { }

        public void DeleteMe() { }

        public void UpdateDatabase() { }

        public bool ValidateCapacity(int slots)
        {
            return true;
        }

        public bool CalidateWeight(int weight)
        {
            return true;
        }
    }
}