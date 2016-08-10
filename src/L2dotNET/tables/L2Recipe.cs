using System.Collections.Generic;

namespace L2dotNET.tables
{
    public class L2Recipe
    {
        public string Name;
        public int RecipeId;
        public int Level;
        public int Iscommonrecipe;
        public int ItemId;
        public int SuccessRate;
        public int MpConsume;

        public List<RecipeItemEntry> Materials = new List<RecipeItemEntry>();
        public List<RecipeItemEntry> Products = new List<RecipeItemEntry>();
        public List<RecipeItemEntry> NpcFee = new List<RecipeItemEntry>();
        public string Mk;
    }
}