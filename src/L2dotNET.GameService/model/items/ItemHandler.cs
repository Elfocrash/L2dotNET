using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using log4net;
using L2dotNET.GameService.Model.Items.Effects;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Items
{
    public class ItemHandler
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ItemHandler));
        private static volatile ItemHandler instance;
        private static readonly object syncRoot = new object();

        public static ItemHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new ItemHandler();
                        }
                    }
                }

                return instance;
            }
        }

        public void Initialize()
        {
            register(new EnchantScrolls());
            register(new Calculator());

            LoadXML();
            log.Info($"ItemHandler: Loaded {effects} effects with {items.Count} items.");
        }

        public SortedList<int, ItemEffect> items = new SortedList<int, ItemEffect>();

        public bool Process(L2Character character, L2Item item)
        {
            if (items.ContainsKey(item.Template.ItemID))
            {
                items[item.Template.ItemID].Use(character, item);
                return true;
            }
            else
                return false;
        }

        public ItemHandler() { }

        private short effects = 0;

        private void register(ItemEffect effect)
        {
            foreach (int id in effect.ids)
                items.Add(id, effect);

            effects++;
        }

        public void LoadXML()
        {
            XElement xml = XElement.Parse(File.ReadAllText(@"scripts\itemhandler.xml"));
            XElement ex = xml.Element("list");

            foreach (var m in ex.Elements())
            {
                if (m.Name == "item")
                {
                    int id = Convert.ToInt32(m.Attribute("id").Value);

                    ItemHandlerScript ih = new ItemHandlerScript(id);

                    if (m.Attribute("effect") != null)
                    {
                        string str = m.Attribute("effect").Value;
                        ih.EffectID = Convert.ToInt32(str.Split('-')[0]);
                        ih.EffectLv = Convert.ToInt32(str.Split('-')[1]);
                    }

                    if (m.Attribute("skill") != null)
                    {
                        string str = m.Attribute("skill").Value;
                        ih.SkillID = Convert.ToInt32(str.Split('-')[0]);
                        ih.SkillLv = Convert.ToInt32(str.Split('-')[1]);
                    }

                    if (m.Attribute("exchange") != null)
                    {
                        string str = m.Attribute("exchange").Value;
                        foreach (string st in str.Split(';'))
                            ih.addExchangeItem(Convert.ToInt32(st.Split('-')[0]), Convert.ToInt64(st.Split('-')[1]));
                    }

                    if (m.Attribute("pet") != null)
                        ih.Pet = Convert.ToBoolean(m.Attribute("pet").Value);

                    if (m.Attribute("player") != null)
                        ih.Player = Convert.ToBoolean(m.Attribute("player").Value);

                    if (m.Attribute("destroy") != null)
                        ih.Destroy = Convert.ToBoolean(m.Attribute("destroy").Value);

                    if (m.Attribute("petId") != null)
                        ih.PetID = Convert.ToInt32(m.Attribute("petId").Value);

                    if (m.Attribute("summonId") != null)
                        ih.SummonID = Convert.ToInt32(m.Attribute("summonId").Value);

                    if (m.Attribute("summonStaticId") != null)
                        ih.SummonStaticID = Convert.ToInt32(m.Attribute("summonStaticId").Value);

                    items.Add(id, ih);
                }
            }
        }
    }
}