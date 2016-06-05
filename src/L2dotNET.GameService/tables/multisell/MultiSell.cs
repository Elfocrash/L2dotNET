using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using log4net;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Npcs;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Tables.Multisell
{
    public class MultiSell
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MultiSell));

        private static volatile MultiSell instance;
        private static readonly object syncRoot = new object();

        public static MultiSell Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new MultiSell();
                        }
                    }
                }

                return instance;
            }
        }

        public void Initialize()
        {
            LoadXml();
        }

        public MultiSell() { }

        public SortedList<int, MultiSellList> lists = new SortedList<int, MultiSellList>();

        public void ShowList(L2Player player, L2Npc npc, int listId)
        {
            if (!lists.ContainsKey(listId))
            {
                player.sendMessage("Multsell list #" + listId + " was not found");
                player.sendActionFailed();
                return;
            }

            MultiSellList list = lists[listId];

            player.LastRequestedMultiSellId = listId;

            if (list.all == 1)
            {
                player.sendPacket(new MultiSellListEx(player, list));
                if (player.CustomMultiSellList != null)
                    player.CustomMultiSellList = null;
            }
            else
            {
                MultiSellList newlist = new MultiSellList();
                newlist.id = list.id;
                L2Item[] pitems = player.getAllWeaponArmorNonQuestItems();
                foreach (MultiSellEntry entry in list.container)
                {
                    MultiSellItem msitem = entry.take[0];

                    if (msitem.template == null)
                        continue;

                    foreach (L2Item item in pitems)
                    {
                        if (item._isEquipped == 1)
                            continue;

                        if (item.Template.ItemID == msitem.id)
                        {
                            MultiSellEntry edentry = new MultiSellEntry();
                            edentry.take.AddRange(entry.take);
                            edentry.give.AddRange(entry.give);

                            edentry.take[0].l2item = item;
                            edentry.give[0].l2item = item;

                            newlist.container.Add(edentry);
                        }
                    }
                }

                MultiSellListEx mlist = new MultiSellListEx(player, newlist);
                player.CustomMultiSellList = newlist;
                player.sendPacket(mlist);
            }
        }

        public void LoadXml()
        {
            XElement xml = XElement.Parse(File.ReadAllText(@"scripts\multisell.xml"));
            XElement ex = xml.Element("list");

            foreach (var m in ex.Elements())
            {
                if (m.Name == "multisell")
                {
                    MultiSellList mlist = new MultiSellList();
                    mlist.id = Convert.ToInt32(m.Attribute("id").Value);
                    mlist.dutyf = Convert.ToByte(m.Attribute("dutyf").Value);
                    mlist.save = Convert.ToByte(m.Attribute("save").Value);
                    mlist.all = Convert.ToByte(m.Attribute("all").Value);

                    foreach (var stp in m.Elements())
                    {
                        if (stp.Name == "entry")
                        {
                            MultiSellEntry entry = new MultiSellEntry();
                            foreach (var its in stp.Elements())
                            {
                                switch (its.Name.LocalName)
                                {
                                    case "give":
                                    {
                                        MultiSellItem item = new MultiSellItem();
                                        item.id = Convert.ToInt32(its.Attribute("id").Value);
                                        item.count = Convert.ToInt64(its.Attribute("count").Value);
                                        if (item.id > 0)
                                        {
                                            item.template = ItemTable.Instance.GetItem(item.id);
                                            if (!item.template.isStackable())
                                                entry.Stackable = 0;
                                        }
                                        entry.give.Add(item);
                                    }
                                        break;
                                    case "take":
                                    {
                                        MultiSellItem item = new MultiSellItem();
                                        item.id = Convert.ToInt32(its.Attribute("id").Value);
                                        item.count = Convert.ToInt64(its.Attribute("count").Value);
                                        if (item.id > 0)
                                            item.template = ItemTable.Instance.GetItem(item.id);
                                        entry.take.Add(item);
                                    }
                                        break;
                                    case "duty":
                                        entry.dutyCount = Convert.ToInt64(its.Attribute("count").Value);
                                        break;
                                }
                            }

                            mlist.container.Add(entry);
                        }
                    }

                    lists.Add(mlist.id, mlist);
                }
            }

            log.Info($"MultiSell: {lists.Count} lists");
        }

        public MultiSellList getList(int listId)
        {
            if (lists.ContainsKey(listId))
                return lists[listId];

            return null;
        }
    }
}