using L2dotNET.GameService.Model.Items;

namespace L2dotNET.GameService.Tables
{
    public class recipe_item_entry
    {
        public ItemTemplate item;
        public long count;
        public double rate;

        public recipe_item_entry(int id, long count)
        {
            item = ItemTable.Instance.GetItem(id);
            this.count = count;
        }
    }
}