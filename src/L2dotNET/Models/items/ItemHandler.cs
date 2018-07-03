using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using L2dotNET.Models.Items.Effects;
using NLog;

namespace L2dotNET.Models.Items
{
    public class ItemHandler
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static short _effects;

        public static Dictionary<int, ItemEffect> Items { get; private set; }

        public static void Initialize()
        {
            Items = new Dictionary<int, ItemEffect>();

            Register(new EnchantScrolls());
            Register(new Calculator());

            LoadXml();
            Log.Info($"Loaded {_effects} effects with {Items.Count} items.");
        }


        public static bool Process(L2Character character, L2Item item)
        {
            if (!Items.ContainsKey(item.Template.ItemId))
            {
                return false;
            }

            Items[item.Template.ItemId].Use(character, item);
            return true;
        }


        private static void Register(ItemEffect effect)
        {
            foreach (int id in effect.Ids)
                Items.Add(id, effect);

            _effects++;
        }

        public static void LoadXml()
        {
            XElement xml = XElement.Parse(File.ReadAllText(@"scripts\itemhandler.xml"));
            XElement ex = xml.Element("list");

            if (ex == null)
            {
                return;
            }

            foreach (XElement m in ex.Elements())
            {
                if (m.Name != "item")
                {
                    continue;
                }

                int id = Convert.ToInt32(m.Attribute("id").Value);

                ItemHandlerScript itemHandler = new ItemHandlerScript(id);

                if (m.Attribute("effect") != null)
                {
                    string str = m.Attribute("effect").Value;
                    itemHandler.EffectId = Convert.ToInt32(str.Split('-')[0]);
                    itemHandler.EffectLvl = Convert.ToInt32(str.Split('-')[1]);
                }

                if (m.Attribute("skill") != null)
                {
                    string str = m.Attribute("skill").Value;
                    itemHandler.SkillId = Convert.ToInt32(str.Split('-')[0]);
                    itemHandler.SkillLvl = Convert.ToInt32(str.Split('-')[1]);
                }

                if (m.Attribute("exchange") != null)
                {
                    string str = m.Attribute("exchange").Value;
                    foreach (string st in str.Split(';'))
                        itemHandler.AddExchangeItem(Convert.ToInt32(st.Split('-')[0]), Convert.ToInt32(st.Split('-')[1]));
                }

                if (m.Attribute("pet") != null)
                {
                    itemHandler.Pet = Convert.ToBoolean(m.Attribute("pet").Value);
                }

                if (m.Attribute("player") != null)
                {
                    itemHandler.Player = Convert.ToBoolean(m.Attribute("player").Value);
                }

                if (m.Attribute("destroy") != null)
                {
                    itemHandler.Destroy = Convert.ToBoolean(m.Attribute("destroy").Value);
                }

                if (m.Attribute("petId") != null)
                {
                    itemHandler.PetId = Convert.ToInt32(m.Attribute("petId").Value);
                }

                if (m.Attribute("summonId") != null)
                {
                    itemHandler.SummonId = Convert.ToInt32(m.Attribute("summonId").Value);
                }

                if (m.Attribute("summonStaticId") != null)
                {
                    itemHandler.SummonStaticId = Convert.ToInt32(m.Attribute("summonStaticId").Value);
                }

                Items.Add(id, itemHandler);
            }
        }
    }
}