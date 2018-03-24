using L2dotNET.Models.items;

namespace L2dotNET.tables
{
    public class RecipeItemEntry
    {
        public ItemTemplate Item;
        public int Count;
        public double Rate;

        public RecipeItemEntry(int id, int count)
        {
            Item = ItemTable.Instance.GetItem(id);
            Count = count;
        }
    }
}