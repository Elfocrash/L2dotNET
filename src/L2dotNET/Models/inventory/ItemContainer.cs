using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using L2dotNET.DataContracts;
using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Models.Items;
using L2dotNET.Models.Player;
using L2dotNET.Services.Contracts;
using L2dotNET.Tables;
using L2dotNET.World;
using Microsoft.Extensions.DependencyInjection;

namespace L2dotNET.Models.Inventory
{
    public abstract class ItemContainer
    {
        protected readonly ICrudService<ItemContract> _itemCrudService;
        protected readonly IItemService _itemService;
        protected readonly IdFactory _idFactory;
        protected readonly ItemTable _itemTable;

        public List<L2Item> Items { get; }

        protected ItemContainer(L2Character owner)
        {
            Owner = owner;

            _itemService = GameServer.ServiceProvider.GetService<IItemService>();
            _itemTable = GameServer.ServiceProvider.GetService<ItemTable>();
            _idFactory = GameServer.ServiceProvider.GetService<IdFactory>();
            _itemCrudService = GameServer.ServiceProvider.GetService<ICrudService<ItemContract>>();

            Items = new List<L2Item>();
        }

        protected L2Character Owner { get; set; }

        protected ItemLocation BaseLocation { get; }

        public int OwnerId => Owner?.ObjectId ?? 0;

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
            return Items.FirstOrDefault(item => item.ObjectId == objectId);
        }

        public virtual async Task Restore(L2Character owner)
        {
            IEnumerable<ItemContract> models = await _itemService.RestoreInventory(owner.ObjectId);
            List<L2Item> items = RestoreFromDb(models.ToList());

            foreach (L2Item item in items)
            {
                L2World.AddObject(item);
                Owner = owner;
                AddItem(item, (L2Player)Owner);
            }
        }
        public List<L2Item> RestoreFromDb(List<ItemContract> models)
        {
            return models.Select(MapModelToItem).ToList();
        }

        private L2Item MapModelToItem(ItemContract contract)
        {
            L2Item item = new L2Item(_itemCrudService, _idFactory, _itemTable.GetItem(contract.ItemId), _idFactory.NextId())
            {
                ObjectId = contract.ObjectId,
                Count = contract.Count,
                CustomType1 = contract.CustomType1,
                CustomType2 = contract.CustomType2,
                Enchant = contract.Enchant,
                SlotLocation = contract.LocationData,
                OwnerId = contract.CharacterId,
            };

            return item;
        }

        public L2Item AddItem(L2Item item, L2Player player)
        {
            if (item != null)
            {
                item.OwnerId = player.ObjectId;
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
                    ItemTemplate template = _itemTable.GetItem(itemId);
                    if (template == null)
                        return null;

                    item = _itemTable.CreateItem(itemId, count, player);
                    item.OwnerId = player.ObjectId;
                    item.SlotLocation = 0;
                    item.ExistsInDb = ExistsInDb;
                    item.Location = ItemLocation.Inventory;
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