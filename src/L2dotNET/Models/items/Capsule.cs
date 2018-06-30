using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using L2dotNET.Models.Player;
using NLog;

namespace L2dotNET.Models.Items
{
    public class Capsule
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static volatile Capsule _instance;
        private static readonly object SyncRoot = new object();

        public static Capsule Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new Capsule();
                }

                return _instance;
            }
        }

        public void Initialize()
        {
            LoadXml();
            Log.Info($"Loaded {Items.Count} items.");
        }

        public SortedList<int, CapsuleItem> Items = new SortedList<int, CapsuleItem>();

        public void Process(L2Character character, L2Item item)
        {
            if (!(character is L2Player))
                return;

            if (!Items.ContainsKey(item.Template.ItemId))
                return;

            CapsuleItem caps = Items[item.Template.ItemId];
            Random rn = new Random();
            ((L2Player)character).DestroyItem(item, 1);
            foreach (CapsuleItemReward rew in caps.Rewards.Where(rew => rn.Next(100) <= rew.Rate))
                ((L2Player)character).AddItem(rew.Id, rn.Next(rew.Min, rew.Max));
        }

        public void LoadXml()
        {
            XElement xml = XElement.Parse(File.ReadAllText(@"scripts\extractable.xml"));
            XElement ex = xml.Element("list");

            if (ex == null)
                return;

            foreach (XElement m in ex.Elements())
            {
                if (m.Name != "capsule")
                    continue;

                CapsuleItem caps = new CapsuleItem
                {
                    Id = Convert.ToInt32(m.Attribute("id").Value)
                };

                foreach (XElement stp in m.Elements())
                {
                    switch (stp.Name.LocalName)
                    {
                        case "item":
                            try
                            {
                                CapsuleItemReward rew = new CapsuleItemReward
                                {
                                    Id = int.Parse(stp.Attribute("id").Value),
                                    Min = int.Parse(stp.Attribute("min").Value),
                                    Max = int.Parse(stp.Attribute("max").Value),
                                    Rate = int.Parse(stp.Attribute("rate").Value)
                                };
                                caps.Rewards.Add(rew);
                            }
                            catch (Exception)
                            {
                                Log.Error($"cant parse capsule {caps.Id}");
                            }
                            break;
                    }
                }

                Items.Add(caps.Id, caps);
            }
        }
    }
}