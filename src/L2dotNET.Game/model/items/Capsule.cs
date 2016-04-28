using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using L2dotNET.Game.world;
using log4net;

namespace L2dotNET.Game.model.items
{
    public class Capsule
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Capsule));
        private static Capsule instance = new Capsule();
        public static Capsule getInstance()
        {
            return instance;
        }

        public SortedList<int, CapsuleItem> items = new SortedList<int, CapsuleItem>();

        public void Process(L2Character character, L2Item item)
        {
            if (!(character is L2Player))
                return;

            if (items.ContainsKey(item.Template.ItemID))
            {
                CapsuleItem caps = items[item.Template.ItemID];
                Random rn = new Random();
                ((L2Player)character).Inventory.destroyItem(item, 1, true, true);
                foreach (CapsuleItemReward rew in caps.rewards)
                {
                    if(rn.Next(100) <= rew.rate)
                        ((L2Player)character).addItem(rew.id, rn.Next(rew.min, rew.max));
                }
            }
        }

        public Capsule()
        {
            loadXML();
            log.Info("Capsule: Loaded " + items.Count + " items.");
        }

        public void loadXML()
        {
            XElement xml = XElement.Parse(File.ReadAllText(@"scripts\extractable.xml"));
            XElement ex = xml.Element("list");

            foreach (var m in ex.Elements())
            {
                if (m.Name == "capsule")
                {
                    CapsuleItem caps = new CapsuleItem();
                    caps.id = Convert.ToInt32(m.Attribute("id").Value);

                    foreach (var stp in m.Elements())
                    {
                        switch (stp.Name.LocalName)
                        {
                            case "item":
                                try
                                {
                                    CapsuleItemReward rew = new CapsuleItemReward();
                                    rew.id = int.Parse(stp.Attribute("id").Value);
                                    rew.min = int.Parse(stp.Attribute("min").Value);
                                    rew.max = int.Parse(stp.Attribute("max").Value);
                                    rew.rate = int.Parse(stp.Attribute("rate").Value);
                                    caps.rewards.Add(rew);
                                }
                                catch (Exception)
                                {
                                    Console.WriteLine("cant parse capsule "+caps.id);
                                }
                                break;
                        }

                    }

                    items.Add(caps.id, caps);
                }
            }
        }
    }

    public class CapsuleItem
    {
        public int id;
        public List<CapsuleItemReward> rewards = new List<CapsuleItemReward>();
    }

    public class CapsuleItemReward
    {
        public int id;
        public int min;
        public int max;
        public int rate;
    }
}
