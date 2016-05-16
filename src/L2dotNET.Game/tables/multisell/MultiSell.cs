using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using L2dotNET.GameService.model.items;
using L2dotNET.GameService.network.l2send;
using L2dotNET.GameService.model.npcs;
using log4net;

namespace L2dotNET.GameService.tables.multisell
{
    public class MultiSell
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MultiSell));

        private static volatile MultiSell instance;
        private static object syncRoot = new object();

        public static MultiSell Instance
        {
            get
            {
                if(instance == null)
                {
                    lock(syncRoot)
                    {
                        if(instance == null)
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

        public MultiSell()
        {

        }

        public SortedList<int, MultiSellList> lists = new SortedList<int, MultiSellList>();

        public void ShowList(L2Player player, L2Citizen npc, int listId)
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

            log.Info($"MultiSell: { lists.Count } lists");
        }

        public MultiSellList getList(int listId)
        {
            if (lists.ContainsKey(listId))
                return lists[listId];

            return null;
        }
    }

    public class MultiSellList
    {
        public int id;
        public byte dutyf = 1;
        public byte save = 0;
        public byte all = 1;
        public readonly List<MultiSellEntry> container = new List<MultiSellEntry>();
    }

    public class MultiSellEntry
    {
        public readonly List<MultiSellItem> take = new List<MultiSellItem>();
        public readonly List<MultiSellItem> give = new List<MultiSellItem>();
        public long dutyCount;
        public byte Stackable = 1;
        public short enchant;
        public short AttrAttackType = -2;
        public short AttrAttackValue;
        public short AttrDefenseValueFire;
        public short AttrDefenseValueWater;
        public short AttrDefenseValueWind;
        public short AttrDefenseValueEarth;
        public short AttrDefenseValueHoly;
        public short AttrDefenseValueUnholy;
    }

    public class MultiSellItem
    {
        public int id;
        public long count;
        public model.items.ItemTemplate template;
        public short enchant
        {
            get
            {
                if (l2item != null)
                    return (short)l2item.Enchant;

                if (template != null && template.enchanted > 0)
                    return template.enchanted;

                return 0;
            }
        }

        public int augment = 0;
        public L2Item l2item;
        public short AttrAttackType
        {
            get
            {
                if (l2item != null)
                    return l2item.AttrAttackType;

                if (template == null)
                    return -2;
                else
                    return template.AttrAttackType;
            }
        }

        public short AttrAttackValue
        {
            get
            {
                if (l2item != null)
                    return l2item.AttrAttackValue;

                if (template == null)
                    return 0;
                else
                    return template.AttrAttackValue;
            }
        }

        public short AttrDefenseValueFire
        {
            get
            {
                if (l2item != null)
                    return l2item.AttrDefenseValueFire;

                if (template == null)
                    return 0;
                else
                    return template.AttrDefenseValueFire;
            }
        }

        public short AttrDefenseValueWater
        {
            get
            {
                if (l2item != null)
                    return l2item.AttrDefenseValueWater;

                if (template == null)
                    return 0;
                else
                    return template.AttrDefenseValueWater;
            }
        }

        public short AttrDefenseValueWind
        {
            get
            {
                if (l2item != null)
                    return l2item.AttrDefenseValueWind;

                if (template == null)
                    return 0;
                else
                    return template.AttrDefenseValueWind;
            }
        }

        public short AttrDefenseValueEarth
        {
            get
            {
                if (l2item != null)
                    return l2item.AttrDefenseValueEarth;

                if (template == null)
                    return 0;
                else
                    return template.AttrDefenseValueEarth;
            }
        }

        public short AttrDefenseValueHoly
        {
            get
            {
                if (l2item != null)
                    return l2item.AttrDefenseValueHoly;

                if (template == null)
                    return 0;
                else
                    return template.AttrDefenseValueHoly;
            }
        }

        public short AttrDefenseValueUnholy
        {
            get
            {
                if (l2item != null)
                    return l2item.AttrDefenseValueUnholy;

                if (template == null)
                    return 0;
                else
                    return template.AttrDefenseValueUnholy;
            }
        }

        public int Durability
        {
            get
            {
                if (l2item != null)
                    return l2item.Template.Durability;

                if (template == null)
                    return 0;
                else
                    return template.Durability;
            }
        }

        public short Type2
        {
            get
            {
                if (template == null)
                    return 0;
                else
                {
                    if (l2item != null)
                        return l2item.Template.Type2();

                    return template.Type2();
                }
            }
        }

        public int BodyPartId
        {
            get
            {
                if (template == null)
                    return 0;
                else
                {
                    if (l2item != null)
                        return l2item.Template.BodyPartId();

                    return template.BodyPartId();
                }
            }
        }

        public int ItemID
        {
            get
            {
                return id;
            }
        }
    }
}
