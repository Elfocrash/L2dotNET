using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using log4net;
using L2dotNET.model.items.effects;
using L2dotNET.world;

namespace L2dotNET.model.items
{
    public class ItemHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ItemHandler));
        private static volatile ItemHandler _instance;
        private static readonly object SyncRoot = new object();

        public static ItemHandler Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new ItemHandler();
                }

                return _instance;
            }
        }

        public void Initialize()
        {
            Register(new EnchantScrolls());
            Register(new Calculator());

            LoadXml();
            Log.Info($"ItemHandler: Loaded {_effects} effects with {Items.Count} items.");
        }

        public SortedList<int, ItemEffect> Items = new SortedList<int, ItemEffect>();

        public bool Process(L2Character character, L2Item item)
        {
            if (!Items.ContainsKey(item.Template.ItemId))
                return false;

            Items[item.Template.ItemId].Use(character, item);
            return true;
        }

        private short _effects;

        private void Register(ItemEffect effect)
        {
            foreach (int id in effect.Ids)
                Items.Add(id, effect);

            _effects++;
        }

        public void LoadXml()
        {
            XElement xml = XElement.Parse(File.ReadAllText(@"scripts\itemhandler.xml"));
            XElement ex = xml.Element("list");

            if (ex == null)
                return;

            foreach (XElement m in ex.Elements())
            {
                if (m.Name != "item")
                    continue;

                int id = Convert.ToInt32(m.Attribute("id").Value);

                ItemHandlerScript ih = new ItemHandlerScript(id);

                if (m.Attribute("effect") != null)
                {
                    string str = m.Attribute("effect").Value;
                    ih.EffectId = Convert.ToInt32(str.Split('-')[0]);
                    ih.EffectLv = Convert.ToInt32(str.Split('-')[1]);
                }

                if (m.Attribute("skill") != null)
                {
                    string str = m.Attribute("skill").Value;
                    ih.SkillId = Convert.ToInt32(str.Split('-')[0]);
                    ih.SkillLv = Convert.ToInt32(str.Split('-')[1]);
                }

                if (m.Attribute("exchange") != null)
                {
                    string str = m.Attribute("exchange").Value;
                    foreach (string st in str.Split(';'))
                        ih.AddExchangeItem(Convert.ToInt32(st.Split('-')[0]), Convert.ToInt32(st.Split('-')[1]));
                }

                if (m.Attribute("pet") != null)
                    ih.Pet = Convert.ToBoolean(m.Attribute("pet").Value);

                if (m.Attribute("player") != null)
                    ih.Player = Convert.ToBoolean(m.Attribute("player").Value);

                if (m.Attribute("destroy") != null)
                    ih.Destroy = Convert.ToBoolean(m.Attribute("destroy").Value);

                if (m.Attribute("petId") != null)
                    ih.PetId = Convert.ToInt32(m.Attribute("petId").Value);

                if (m.Attribute("summonId") != null)
                    ih.SummonId = Convert.ToInt32(m.Attribute("summonId").Value);

                if (m.Attribute("summonStaticId") != null)
                    ih.SummonStaticId = Convert.ToInt32(m.Attribute("summonStaticId").Value);

                Items.Add(id, ih);
            }
        }
    }
}