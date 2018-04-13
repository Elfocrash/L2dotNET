using L2dotNET.Models.Items;

namespace L2dotNET.Tables
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