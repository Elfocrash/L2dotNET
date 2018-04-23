using L2dotNET.Models.Items;

namespace L2dotNET.Tables
{
    public class RecipeItemEntry
    {
        public ItemTemplate Item;
        public int Count;
        public double Rate;

        private readonly ItemTable _itemTable;

        public RecipeItemEntry(ItemTable itemTable)
        {
            _itemTable = itemTable;
        }

        public RecipeItemEntry(int id, int count, ItemTable itemTable)
        {
            Item = _itemTable.GetItem(id);
            Count = count;
            _itemTable = itemTable;
        }
    }
}