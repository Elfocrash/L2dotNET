using L2dotNET.GameService.Model.Items;

namespace L2dotNET.GameService.Tables
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