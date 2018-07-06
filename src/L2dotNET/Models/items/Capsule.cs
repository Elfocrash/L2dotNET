using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using L2dotNET.Models.Player;
using L2dotNET.Utility;
using Mapster;
using NLog;

namespace L2dotNET.Models.Items
{
    public static class Capsule
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public static Dictionary<int, CapsuleItem> Items { get; private set; }

        public static void Initialize()
        {
            Items = new Dictionary<int, CapsuleItem>();
            LoadXml();
            Log.Info($"Loaded {Items.Count} capsule items.");
        }

        public static void Process(L2Character character, L2Item item)
        {
            if (!(character is L2Player) || !Items.ContainsKey(item.Template.ItemId))
            {
                return;
            }

            CapsuleItem caps = Items[item.Template.ItemId];
            L2Player player = (L2Player) character;

            player.DestroyItem(item, 1);
            foreach (CapsuleItemReward rew in caps.Rewards.Where(rew => RandomThreadSafe.Instance.Next(100) <= rew.Rate))
                player.AddItem(rew.Id, RandomThreadSafe.Instance.Next(rew.Min, rew.Max));
        }

        public static void LoadXml()
        {
            XElement xml = XElement.Parse(File.ReadAllText(@"scripts\extractable.xml"));
            XElement ex = xml.Element("list");

            if (ex == null)
            {
                return;
            }

            foreach (XElement m in ex.Elements())
            {
                if (m.Name != "capsule")
                {
                    continue;
                }

                CapsuleItem caps = new CapsuleItem(Convert.ToInt32(m.Attribute("id").Value));

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