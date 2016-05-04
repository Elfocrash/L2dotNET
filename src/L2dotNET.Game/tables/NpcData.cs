using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using L2dotNET.Game.model.items;
using L2dotNET.Game.model.npcs;
using L2dotNET.Game.network.l2send;
using L2dotNET.Game.tables.admin_bypass;
using L2dotNET.Game.tables.ndextend;
using L2dotNET.Game.network;
using log4net;

namespace L2dotNET.Game.tables
{
    class NpcData
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NpcData));
        private static NpcData it = new NpcData();
        public static NpcData getInstance()
        {
            return it;
        }

        public SortedList<int, ND_shop> _shops = new SortedList<int, ND_shop>();
        private NDTeleport Teleports;

        public NpcData()
        {
            load();
        }

        private void load()
        {
            _shops = new SortedList<int, ND_shop>();

            Teleports = new NDTeleport();

            ItemTable itable = ItemTable.getInstance();
            {
                XElement xml = XElement.Parse(File.ReadAllText(@"scripts\buylists.xml"));
                foreach (var shops in xml.Elements("shops"))
                {
                    foreach (var shopp in shops.Elements("shop"))
                    {
                        ND_shop shop = new ND_shop();
                        shop.id = int.Parse(shopp.Element("npc").Value);
                        shop.mod = double.Parse(shopp.Element("mod").Value);

                        foreach (var selllist in shopp.Elements("selllist"))
                        {
                            ND_shopList slist = new ND_shopList();
                            slist.id = short.Parse(selllist.Attribute("id").Value);

                            string items = selllist.Element("item").Value;
                            items = items.Replace("\n", "").Replace(" ", "");

                            foreach (string i in items.Split(','))
                            {
                                ItemTemplate it = itable.getItem(Convert.ToInt32(i));
                                if (it != null)
                                {
                                    slist.items.Add(new ND_shopItem(it));
                                }
                                else
                                    log.Error($"NpcData: cant find item to trade { i } on npc { shop.id }");
                            }

                            shop.lists.Add(slist.id, slist);
                        }

                        _shops.Add(shop.id, shop);
                    }
                }
            }

            log.Info("NpcData: loaded " + _shops.Count + " merchants.");
            //CLogger.info("NpcData: loaded " + _mults.Count + " multisell lists.");
        }

        public void buylist(L2Player player, L2Citizen trader, short reply)
        {
            if (!_shops.ContainsKey(trader.Template.NpcId))
            {
                player.sendMessage("you shop was not found");
                player.sendActionFailed();
                return;
            }

            ND_shop shop = _shops[trader.Template.NpcId];
            GameServerNetworkPacket pk;
            if (!shop.lists.ContainsKey(reply))
            {
                reply -= 2; // примерка

                if (!shop.lists.ContainsKey(reply))
                {
                    player.sendMessage("your shop id was just wrong " + reply);
                    player.sendActionFailed();
                    return;
                }
                else
                    pk = new ShopPreviewList(player, shop.lists[reply], reply);
            }
            else
            {
                player.sendPacket(new ExBuySellList_Buy(player, shop.lists[reply], 1.10, 1.0, reply));
                player.sendPacket(new ExBuySellList_Sell(player));
            }
        }


        public void RequestTeleportList(L2Citizen npc, L2Player player, int groupId)
        {
            RequestTeleportList(npc, player, groupId, -1);
        }

        public void RequestTeleportList(L2Citizen npc, L2Player player, int groupId, int itemId)
        {
            if (!Teleports.npcs.ContainsKey(npc.Template.NpcId))
            {
                player.ShowHtmPlain("no teleports available for you", npc);
                player.sendActionFailed();
                return;
            }

            ab_teleport_group group = Teleports.npcs[npc.Template.NpcId].groups[groupId];
            StringBuilder sb = new StringBuilder("&$556;<br><br>");
            foreach (ab_teleport_entry e in group._teles.Values)
            {
                string cost = "";
                int id = itemId != -1 ? itemId : e.itemId;
                if (player.Level >= 40)
                    cost = " - " + e.cost + " &#" + id + ";";

                sb.Append("<a action=\"bypass -h teleport_next?ask=" + groupId + "&reply=" + e.id + "\" msg=\"811;" + e.name + "\">" + e.name + "" + cost + "</a><br1>");
            }

            player.TeleportPayID = itemId;
            player.ShowHtmPlain(sb.ToString(), npc);
        }

        public void RequestTeleport(L2Citizen npc, L2Player player, int type, int entryId)
        {
            ab_teleport_group group = null;
            try
            {
                group = Teleports.npcs[npc.Template.NpcId].groups[type];
            }
            catch
            {
                log.Error($"ND:RequestTeleport cant find teleport group { type }");
                player.sendActionFailed();
                return;
            }

            ab_teleport_entry e = group._teles[entryId];

            if (!player.hasItem(e.itemId, e.cost))
            {
                switch (e.itemId)
                {
                    case 57:
                        player.sendSystemMessage(279); //You do not have enough adena.
                        break;
                    case 6651:
                        player.ShowHtm("fornonoblessitem.htm", npc);
                        break;

                    default:
                        player.sendSystemMessage(701); //You do not have enough required items.
                        break;
                }

                player.sendActionFailed();
                return;
            }

            switch (e.itemId)
            {
                case 57:
                    player.reduceAdena(e.cost, true, true);
                    break;

                default:
                    player.Inventory.destroyItem(e.itemId, e.cost, true, true);
                    break;
            }

            player.teleport(e.x, e.y, e.z);
        }

        internal void preview(L2Player talker, L2Citizen myself, int p)
        {
            throw new NotImplementedException();
        }
    }

    public class ND_shop
    {
        public SortedList<short, ND_shopList> lists = new SortedList<short, ND_shopList>();
        public double mod;
        public int id;
    }

    public class ND_shopList
    {
        public List<ND_shopItem> items = new List<ND_shopItem>();
        public short id;
    }

    public class ND_shopItem
    {
        public ItemTemplate item;
        public int count = -1;

        public ND_shopItem(ItemTemplate it)
        {
            this.item = it;
        }
    }
}
