using L2dotNET.GameService.Model.Items;

namespace L2dotNET.GameService.Tables
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