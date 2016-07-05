using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using log4net;

namespace L2dotNET.GameService.Tables
{
    class RecipeTable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RecipeTable));
        private static volatile RecipeTable _instance;
        private static readonly object SyncRoot = new object();

        public static RecipeTable Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new RecipeTable();
                        }
                    }
                }

                return _instance;
            }
        }

        public void Initialize()
        {
            XElement xml = XElement.Parse(File.ReadAllText(@"scripts\recipes.xml"));
            XElement ex = xml.Element("list");
            if (ex != null)
            {
                foreach (XElement m in ex.Elements())
                    if (m.Name == "recipe")
                    {
                        L2Recipe rec = new L2Recipe();
                        rec.RecipeId = int.Parse(m.Attribute("id").Value);
                        rec.Mk = m.Attribute("mk").Value;
                        rec.Level = int.Parse(m.Attribute("level").Value);
                        rec.ItemId = int.Parse(m.Attribute("itemId").Value);
                        rec.Iscommonrecipe = int.Parse(m.Attribute("common").Value);

                        foreach (XElement stp in m.Elements())
                            switch (stp.Name.LocalName)
                            {
                                case "material":
                                {
                                    rec.MpConsume = int.Parse(stp.Attribute("mp").Value);
                                    foreach (XElement items in stp.Elements().Where(items => items.Name == "item"))
                                    {
                                        RecipeItemEntry item = new RecipeItemEntry(int.Parse(items.Attribute("id").Value), int.Parse(items.Attribute("count").Value));
                                        rec.Materials.Add(item);
                                    }
                                }

                                    break;
                                case "product":
                                {
                                    rec.SuccessRate = int.Parse(stp.Attribute("rate").Value);
                                    foreach (XElement items in stp.Elements().Where(items => items.Name == "item"))
                                    {
                                        RecipeItemEntry item = new RecipeItemEntry(int.Parse(items.Attribute("id").Value), int.Parse(items.Attribute("count").Value));
                                        item.Rate = double.Parse(items.Attribute("rate").Value);
                                        rec.Products.Add(item);
                                    }
                                }

                                    break;
                                case "fee":
                                {
                                    foreach (XElement items in stp.Elements().Where(items => items.Name == "item"))
                                    {
                                        RecipeItemEntry item = new RecipeItemEntry(int.Parse(items.Attribute("id").Value), int.Parse(items.Attribute("count").Value));
                                        rec.NpcFee.Add(item);
                                    }
                                }

                                    break;
                            }

                        Recipes.Add(rec.RecipeId, rec);
                    }
            }

            Log.Info("RecipeTable: loaded " + Recipes.Count + " recipes.");
        }

        public readonly SortedList<int, L2Recipe> Recipes = new SortedList<int, L2Recipe>();

        public L2Recipe GetById(int p)
        {
            return !Recipes.ContainsKey(p) ? null : Recipes[p];
        }

        public L2Recipe GetByItemId(int p)
        {
            return Recipes.Values.FirstOrDefault(rec => rec.ItemId == p);
        }
    }
}