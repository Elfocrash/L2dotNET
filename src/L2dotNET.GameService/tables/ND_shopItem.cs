using L2dotNET.GameService.Model.Items;

namespace L2dotNET.GameService.Tables
{
    public class ND_shopItem
    {
        public ItemTemplate item;
        public int count = -1;

        public ND_shopItem(ItemTemplate it)
        {
            item = it;
        }
    }
}