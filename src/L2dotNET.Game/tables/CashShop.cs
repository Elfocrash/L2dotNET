using System.Collections.Generic;
using L2dotNET.Game.model.items;
using L2dotNET.Game.network.l2send;

namespace L2dotNET.Game.tables
{
    class CashShop
    {
        private static CashShop instance = new CashShop();
        public static CashShop getInstance()
        {
            return instance;
        }

        public int BR_BUY_SUCCESS = 1; //The item has been successfully purchased.
        public int BR_BUY_LACK_OF_POINT = -1; //Game points are not enough.
        public int BR_BUY_INVALID_PRODUCT = -2; //Product Purchase Error - The product is not right.
        public int BR_BUY_USER_CANCEL = -3; //The item has failed to be purchased.
        public int BR_BUY_INVENTROY_OVERFLOW = -4; //The item cannot be received because the inventory weight/quantity limit has been exceeded.
        public int BR_BUY_CLOSED_PRODUCT = -5; //Product Purchase Error - The product is not right.
        public int BR_BUY_SERVER_ERROR = -6; //The item has failed to be purchased.
        public int BR_BUY_BEFORE_SALE_DATE = -7; //The item you selected cannot be purchased. Unfortunately, the sale period ended.
        public int BR_BUY_AFTER_SALE_DATE = -8; //The item you selected cannot be purchased. Unfortunately, the sale period ended.
        public int BR_BUY_INVALID_USER = -9; //Product Purchase Error - The user state is not right.
        public int BR_BUY_INVALID_ITEM = -10; //Product Purchase Error - The item within the product is not right.
        public int BR_BUY_INVALID_USER_STATE = -11; //Product Purchase Error - The user state is not right.
        public int BR_BUY_NOT_DAY_OF_WEEK = -12; //You cannot buy the item on this day of the week.
        public int BR_BUY_NOT_TIME_OF_DAY = -13; //You cannot buy the item at this hour.
        public int BR_BUY_SOLD_OUT = -14; //Item out of stock.

        public int MAX_BUY_COUNT = 99;

        public SortedList<int, CashShopItemTemplate> _templates = new SortedList<int, CashShopItemTemplate>();
        public SortedList<int, CashShopItem> _items = new SortedList<int, CashShopItem>();

        public CashShop()
        {
            CashShopItemTemplate t = new CashShopItemTemplate();
            t.ClientID = 1080001;
            t.Item = ItemTable.getInstance().getItem(22000);
            t.Category = 5;
            CashShopItem item = new CashShopItem(t);
            item.Count = 1;
            item.Price = 250;
            _items.Add(item.Template.ClientID, item);

            CashShopItemTemplate t2 = new CashShopItemTemplate();
            t2.ClientID = 1080039;
            t2.Item = ItemTable.getInstance().getItem(22057);
            t2.Category = 5;
            CashShopItem item2 = new CashShopItem(t2);
            item2.Count = 1;
            item2.Price = 600;

            _items.Add(item2.Template.ClientID, item2);
        }

        public void sendResult(L2Player player, int result)
        {
            player.sendPacket(new ExBR_BuyProduct(result));
        }

        public void showItemInfo(L2Player player, int clientID)
        {
            if (!_items.ContainsKey(clientID))
            {
                sendResult(player, BR_BUY_INVALID_ITEM);
                return;
            }

            player.sendPacket(new ExBR_ProductInfo(_items[clientID]));
        }

        public void showList(L2Player player)
        {
            player.sendPacket(new ExBR_ProductList(_items.Values));
        }

        public void showRecentList(L2Player player)
        {
            List<CashShopItem> recent = new List<CashShopItem>();
            foreach (int id in player.CashShopRecent)
                if (_items.ContainsKey(id))
                    recent.Add(_items[id]);

            player.sendPacket(new ExBR_RecentProductList(recent));
        }

        public void requestBuyItem(L2Player player, int clientID, int count)
        {
            if (count > MAX_BUY_COUNT)
                count = MAX_BUY_COUNT;
            if (count < 1)
                count = 1;

            if (!_items.ContainsKey(clientID))
            {
                sendResult(player, BR_BUY_INVALID_ITEM);
                return;
            }

            CashShopItem item = _items[clientID];

            if (player.ExtraPoints < (item.Price * count))
            {
                sendResult(player, BR_BUY_LACK_OF_POINT);
                return;
            }

            //if (item.Stock - item.MaxStock < 1)
            //{
            //    sendResult(player, BR_BUY_SOLD_OUT);
            //    return;
            //}

            if (!player.CheckFreeWeight(item.Template.Item.Weight * count))
            {
                sendResult(player, BR_BUY_INVENTROY_OVERFLOW);
                return;
            }

            if (!player.CheckFreeSlotsInventory(item.Template.Item, count))
            {
                sendResult(player, BR_BUY_INVENTROY_OVERFLOW);
                return;
            }

            player.ExtraPoints = player.ExtraPoints - (item.Price * count);
            player.sendPacket(new ExBR_GamePoint(player.ObjID, player.ExtraPoints));

            player.Inventory.addItem(item.Template.Item, count, 0, true, true);
            sendResult(player, BR_BUY_SUCCESS);

            if (!player.CashShopRecent.Contains(item.Template.ClientID))
                player.CashShopRecent.Add(item.Template.ClientID);
        }
    }

    public class CashShopItem
    {
        public CashShopItemTemplate Template;
        public int Count, Price;

        public int DayWeek;
        public int StartSale;
        public int EndSale = 2127441600;
        public int StartHour;
        public int StartMin;
        public int EndHour = 23;
        public int EndMin = 59;
        public int Stock;
        public int MaxStock;
        public bool Limited;
        // 1 - event, 2 - best, 3 - event+best
        public int IsEventItem;

        public CashShopItem(CashShopItemTemplate t)
        {
            Template = t;
        }
    }

    public class CashShopItemTemplate
    {
        public int ClientID, Category;
        public ItemTemplate Item;
    }
}
