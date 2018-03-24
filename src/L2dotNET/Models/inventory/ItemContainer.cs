using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using L2dotNET.DataContracts;
using L2dotNET.model.items;
using L2dotNET.model.player;
using L2dotNET.Services.Contracts;
using L2dotNET.tables;
using L2dotNET.world;
using Ninject;

namespace L2dotNET.model.inventory
{
    [Synchronization]
    public abstract class ItemContainer
    {
        [Inject]
        public IItemService ItemService => GameServer.Kernel.Get<IItemService>();

        public List<L2Item> Items;

        protected ItemContainer()
        {
            Items = new List<L2Item>();
        }

        protected abstract L2Character Owner { get; set; }

        protected abstract L2Item.ItemLocation BaseLocation { get; }

        public int OwnerId => Owner?.ObjId ?? 0;

        public int Count =>Items.Count;

        public string Name => "ItemContainer";

        public bool HasAtLeaseOneItem(params int[] itemIds)
        {
            return itemIds.Any(itemId => GetItemsByItemId(itemId) != null);
        }

        public List<L2Item> GetItemsByItemId(int itemId)
        {
            return Items.Where(item => item.Template.ItemId == itemId).ToList();
        }

        public L2Item GetItemByItemId(int itemId)
        {
            return Items.FirstOrDefault(item => item.Template.ItemId == itemId);
        }

        public L2Item GetItemByObjectId(int objectId)
        {
            return Items.FirstOrDefault(item => item.ObjId == objectId);
        }

        public virtual void Restore(L2Character owner)
        {
            List<ItemContract> models = ItemService.RestoreInventory(owner.ObjId, "Inventory");
            List<L2Item> items = L2Item.RestoreFromDb(models);

            foreach (L2Item item in items)
            {
                L2World.Instance.AddObject(item);
                Owner = owner;
                AddItem(item, (L2Player)Owner);
            }
        }

        public L2Item AddItem(L2Item item, L2Player player)
        {
            if (item != null)
            {
                item.OwnerId = player.ObjId;
                //item.SlotLocation = 0;
                Items.Add(item);

                item.UpdateDatabase();
            }
            return item;
        }

        public L2Item AddItem(int itemId, int count, L2Player player,bool ExistsInDb = false)
        {
            L2Item item = GetItemByItemId(itemId);
            if ((item != null) && item.Template.Stackable)
            {
                item.ChangeCount(count,player);
                item.UpdateDatabase();
            }
            else
            {
                //for (int i = 0; i < Count; i++)
               // {
                    ItemTemplate template = ItemTable.Instance.GetItem(itemId);
                    if (template == null)
                        return null;

                    item = ItemTable.Instance.CreateItem(itemId, count, player);
                    item.OwnerId = player.ObjId;
                    item.SlotLocation = 0;
                    item.ExistsInDb = ExistsInDb;
                    item.Location = L2Item.ItemLocation.Inventory;
                    Items.Add(item);

                    item.UpdateDatabase();
               // }
            }

            return item;
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
            Items.ForEach(item => DestroyItem(item, item.Count, player));
        }

        public int AdenaCount()
        {
            return Items.Where(item => item.Template.ItemId == 57).Select(item => item.Count).FirstOrDefault();
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