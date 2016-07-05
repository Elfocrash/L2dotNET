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
        private static readonly ILog Log = LogManager.GetLogger(typeof(MultiSell));

        private static volatile MultiSell _instance;
        private static readonly object SyncRoot = new object();

        public static MultiSell Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new MultiSell();
                        }
                    }
                }

                return _instance;
            }
        }

        public void Initialize()
        {
            LoadXml();
        }

        public SortedList<int, MultiSellList> Lists = new SortedList<int, MultiSellList>();

        public void ShowList(L2Player player, L2Npc npc, int listId)
        {
            if (!Lists.ContainsKey(listId))
            {
                player.SendMessage("Multsell list #" + listId + " was not found");
                player.SendActionFailed();
                return;
            }

            MultiSellList list = Lists[listId];

            player.LastRequestedMultiSellId = listId;

            if (list.All == 1)
            {
                player.SendPacket(new MultiSellListEx(player, list));
                player.CustomMultiSellList = null;
            }
            else
            {
                MultiSellList newlist = new MultiSellList();
                newlist.Id = list.Id;
                L2Item[] pitems = player.GetAllItems().ToArray();
                foreach (MultiSellEntry entry in list.Container)
                {
                    MultiSellItem msitem = entry.Take[0];

                    if (msitem.Template == null)
                    {
                        continue;
                    }

                    foreach (L2Item item in pitems)
                    {
                        if (item.IsEquipped == 1)
                        {
                            continue;
                        }

                        if (item.Template.ItemId == msitem.Id)
                        {
                            MultiSellEntry edentry = new MultiSellEntry();
                            edentry.Take.AddRange(entry.Take);
                            edentry.Give.AddRange(entry.Give);

                            edentry.Take[0].L2Item = item;
                            edentry.Give[0].L2Item = item;

                            newlist.Container.Add(edentry);
                        }
                    }
                }

                MultiSellListEx mlist = new MultiSellListEx(player, newlist);
                player.CustomMultiSellList = newlist;
                player.SendPacket(mlist);
            }
        }

        public void LoadXml()
        {
            XElement xml = XElement.Parse(File.ReadAllText(@"scripts\multisell.xml"));
            XElement ex = xml.Element("list");

            if (ex != null)
            {
                foreach (XElement m in ex.Elements())
                    if (m.Name == "multisell")
                    {
                        MultiSellList mlist = new MultiSellList();
                        mlist.Id = Convert.ToInt32(m.Attribute("id").Value);
                        mlist.Dutyf = Convert.ToByte(m.Attribute("dutyf").Value);
                        mlist.Save = Convert.ToByte(m.Attribute("save").Value);
                        mlist.All = Convert.ToByte(m.Attribute("all").Value);

                        foreach (XElement stp in m.Elements())
                            if (stp.Name == "entry")
                            {
                                MultiSellEntry entry = new MultiSellEntry();
                                foreach (XElement its in stp.Elements())
                                    switch (its.Name.LocalName)
                                    {
                                        case "give":
                                        {
                                            MultiSellItem item = new MultiSellItem();
                                            item.Id = Convert.ToInt32(its.Attribute("id").Value);
                                            item.Count = Convert.ToInt32(its.Attribute("count").Value);
                                            if (item.Id > 0)
                                            {
                                                item.Template = ItemTable.Instance.GetItem(item.Id);
                                                if (!item.Template.IsStackable())
                                                {
                                                    entry.Stackable = 0;
                                                }
                                            }
                                            entry.Give.Add(item);
                                        }
                                            break;
                                        case "take":
                                        {
                                            MultiSellItem item = new MultiSellItem();
                                            item.Id = Convert.ToInt32(its.Attribute("id").Value);
                                            item.Count = Convert.ToInt32(its.Attribute("count").Value);
                                            if (item.Id > 0)
                                            {
                                                item.Template = ItemTable.Instance.GetItem(item.Id);
                                            }
                                            entry.Take.Add(item);
                                        }
                                            break;
                                        case "duty":
                                            entry.DutyCount = Convert.ToInt64(its.Attribute("count").Value);
                                            break;
                                    }

                                mlist.Container.Add(entry);
                            }

                        Lists.Add(mlist.Id, mlist);
                    }
            }

            Log.Info($"MultiSell: {Lists.Count} lists");
        }

        public MultiSellList GetList(int listId)
        {
            return Lists.ContainsKey(listId) ? Lists[listId] : null;
        }
    }
}