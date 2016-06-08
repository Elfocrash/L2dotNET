using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using log4net;

namespace L2dotNET.GameService.Tables
{
    class RecipeTable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RecipeTable));
        private static volatile RecipeTable instance;
        private static readonly object syncRoot = new object();

        public static RecipeTable Instance
        {
            get
            {
                if (instance == null)
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new RecipeTable();
                    }

                return instance;
            }
        }

        public void Initialize()
        {
            XElement xml = XElement.Parse(File.ReadAllText(@"scripts\recipes.xml"));
            XElement ex = xml.Element("list");
            if (ex != null)
                foreach (XElement m in ex.Elements())
                    if (m.Name == "recipe")
                    {
                        L2Recipe rec = new L2Recipe();
                        rec.RecipeID = int.Parse(m.Attribute("id").Value);
                        rec.mk = m.Attribute("mk").Value;
                        rec._level = int.Parse(m.Attribute("level").Value);
                        rec._item_id = int.Parse(m.Attribute("itemId").Value);
                        rec._iscommonrecipe = int.Parse(m.Attribute("common").Value);

                        foreach (XElement stp in m.Elements())
                            switch (stp.Name.LocalName)
                            {
                                case "material":
                                {
                                    rec._mp_consume = int.Parse(stp.Attribute("mp").Value);
                                    foreach (XElement items in stp.Elements().Where(items => items.Name == "item"))
                                    {
                                        recipe_item_entry item = new recipe_item_entry(int.Parse(items.Attribute("id").Value), long.Parse(items.Attribute("count").Value));
                                        rec._materials.Add(item);
                                    }
                                }

                                    break;
                                case "product":
                                {
                                    rec._success_rate = int.Parse(stp.Attribute("rate").Value);
                                    foreach (XElement items in stp.Elements().Where(items => items.Name == "item"))
                                    {
                                        recipe_item_entry item = new recipe_item_entry(int.Parse(items.Attribute("id").Value), long.Parse(items.Attribute("count").Value));
                                        item.rate = double.Parse(items.Attribute("rate").Value);
                                        rec._products.Add(item);
                                    }
                                }

                                    break;
                                case "fee":
                                {
                                    foreach (XElement items in stp.Elements().Where(items => items.Name == "item"))
                                    {
                                        recipe_item_entry item = new recipe_item_entry(int.Parse(items.Attribute("id").Value), long.Parse(items.Attribute("count").Value));
                                        rec._npcFee.Add(item);
                                    }
                                }

                                    break;
                            }

                        _recipes.Add(rec.RecipeID, rec);
                    }

            log.Info("RecipeTable: loaded " + _recipes.Count + " recipes.");
        }

        public readonly SortedList<int, L2Recipe> _recipes = new SortedList<int, L2Recipe>();

        public RecipeTable() { }

        public L2Recipe GetById(int p)
        {
            return !_recipes.ContainsKey(p) ? null : _recipes[p];
        }

        public L2Recipe GetByItemId(int p)
        {
            return _recipes.Values.FirstOrDefault(rec => rec._item_id == p);
        }
    }
}