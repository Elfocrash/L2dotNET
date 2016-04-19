using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using L2dotNET.Game.logger;
using L2dotNET.Game.model.items;

namespace L2dotNET.Game.tables
{
    class RecipeTable
    {
        private static RecipeTable it = new RecipeTable();
        public static RecipeTable getInstance()
        {
            return it;
        }

        public readonly SortedList<int, L2Recipe> _recipes = new SortedList<int, L2Recipe>();

        public RecipeTable()
        {
            XElement xml = XElement.Parse(File.ReadAllText(@"scripts\recipes.xml"));
            XElement ex = xml.Element("list");
            foreach (var m in ex.Elements())
            {
                if (m.Name == "recipe")
                {
                    L2Recipe rec = new L2Recipe();
                    rec.RecipeID = int.Parse(m.Attribute("id").Value);
                    rec.mk = m.Attribute("mk").Value;
                    rec._level = int.Parse(m.Attribute("level").Value);
                    rec._item_id = int.Parse(m.Attribute("itemId").Value);
                    rec._iscommonrecipe = int.Parse(m.Attribute("common").Value);

                    foreach (var stp in m.Elements())
                    {
                        switch (stp.Name.LocalName)
                        {
                            case "material":
                                {
                                    rec._mp_consume = int.Parse(stp.Attribute("mp").Value);
                                    foreach (var items in stp.Elements())
                                    {
                                        if (items.Name == "item")
                                        {
                                            recipe_item_entry item = new recipe_item_entry(int.Parse(items.Attribute("id").Value), long.Parse(items.Attribute("count").Value));
                                            rec._materials.Add(item);
                                        }
                                    }
                                }
                                break;
                            case "product":
                                {
                                    rec._success_rate = int.Parse(stp.Attribute("rate").Value);
                                    foreach (var items in stp.Elements())
                                    {
                                        if (items.Name == "item")
                                        {
                                            recipe_item_entry item = new recipe_item_entry(int.Parse(items.Attribute("id").Value), long.Parse(items.Attribute("count").Value));
                                            item.rate = double.Parse(items.Attribute("rate").Value);
                                            rec._products.Add(item);
                                        }
                                    }
                                }
                                break;
                            case "fee":
                                {
                                    foreach (var items in stp.Elements())
                                    {
                                        if (items.Name == "item")
                                        {
                                            recipe_item_entry item = new recipe_item_entry(int.Parse(items.Attribute("id").Value), long.Parse(items.Attribute("count").Value));
                                            rec._npcFee.Add(item);
                                        }
                                    }
                                }
                                break;
                        }
                    }

                    _recipes.Add(rec.RecipeID, rec);
                }
            }

            CLogger.info("RecipeTable: loaded " + _recipes.Count + " recipes.");
        }

        public L2Recipe getById(int p)
        {
            if (!_recipes.ContainsKey(p))
                return null;

            return _recipes[p];
        }

        public L2Recipe getByItemId(int p)
        {
            foreach (L2Recipe rec in _recipes.Values)
            {
                if (rec._item_id == p)
                    return rec;
            }

            return null;
        }
    }

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

    public class recipe_item_entry
    {
        public ItemTemplate item;
        public long count;
        public double rate;

        public recipe_item_entry(int id, long count)
        {
            item = ItemTable.getInstance().getItem(id);
            this.count = count;
        }
    }
}
