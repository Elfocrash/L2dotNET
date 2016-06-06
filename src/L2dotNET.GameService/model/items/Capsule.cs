using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using log4net;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Items
{
    public class Capsule
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Capsule));
        private static volatile Capsule instance;
        private static readonly object syncRoot = new object();

        public static Capsule Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new Capsule();
                        }
                    }
                }

                return instance;
            }
        }

        public void Initialize()
        {
            LoadXML();
            log.Info($"Capsule: Loaded {items.Count} items.");
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
                    if (rn.Next(100) <= rew.rate)
                        ((L2Player)character).addItem(rew.id, rn.Next(rew.min, rew.max));
                }
            }
        }

        public Capsule() { }

        public void LoadXML()
        {
            XElement xml = XElement.Parse(File.ReadAllText(@"scripts\extractable.xml"));
            XElement ex = xml.Element("list");

            if (ex != null)
            {
                foreach (XElement m in ex.Elements())
                {
                    if (m.Name == "capsule")
                    {
                        CapsuleItem caps = new CapsuleItem();
                        caps.id = Convert.ToInt32(m.Attribute("id").Value);

                        foreach (XElement stp in m.Elements())
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
                                        log.Error("cant parse capsule " + caps.id);
                                    }
                                    break;
                            }
                        }

                        items.Add(caps.id, caps);
                    }
                }
            }
        }
    }
}