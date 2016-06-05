using System.Collections.Generic;

namespace L2dotNET.GameService.Tables
{
    public class L2Recipe
    {
        public string name;
        public int RecipeID;
        public int _level;
        public int _iscommonrecipe;
        public int _item_id;
        public int _success_rate;
        public int _mp_consume;

        public List<recipe_item_entry> _materials = new List<recipe_item_entry>();
        public List<recipe_item_entry> _products = new List<recipe_item_entry>();
        public List<recipe_item_entry> _npcFee = new List<recipe_item_entry>();
        public string mk;
    }
}