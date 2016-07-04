using L2dotNET.GameService.Model.Items;

namespace L2dotNET.GameService.Tables
{
    public class NdShopItem
    {
        public ItemTemplate Item;
        public int Count = -1;

        public NdShopItem(ItemTemplate it)
        {
            Item = it;
        }
    }
}