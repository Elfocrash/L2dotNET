using L2dotNET.model.items;

namespace L2dotNET.tables
{
    public class NDShopItem
    {
        public ItemTemplate Item;
        public int Count = -1;

        public NDShopItem(ItemTemplate it)
        {
            Item = it;
        }
    }
}