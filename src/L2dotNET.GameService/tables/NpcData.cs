using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using log4net;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Npcs;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.Tables.Admin_Bypass;
using L2dotNET.GameService.Tables.Ndextend;

namespace L2dotNET.GameService.Tables
{
    class NpcData
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(NpcData));
        private static volatile NpcData _instance;
        private static readonly object SyncRoot = new object();

        public static NpcData Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new NpcData();
                }

                return _instance;
            }
        }

        public void Initialize()
        {
            Load();
        }

        public SortedList<int, NDShop> Shops = new SortedList<int, NDShop>();
        private NDTeleport _teleports;

        private void Load()
        {
            Shops = new SortedList<int, NDShop>();

            _teleports = new NDTeleport();

            ItemTable itable = ItemTable.Instance;
            {
                XElement xml = XElement.Parse(File.ReadAllText(@"scripts\buylists.xml"));
                foreach (XElement shops in xml.Elements("shops"))
                    foreach (XElement shopp in shops.Elements("shop"))
                    {
                        NDShop shop = new NDShop();
                        XElement npcElement = shopp.Element("npc");
                        if (npcElement != null)
                            shop.Id = int.Parse(npcElement.Value);
                        XElement modElement = shopp.Element("mod");
                        if (modElement != null)
                            shop.Mod = double.Parse(modElement.Value);

                        foreach (XElement selllist in shopp.Elements("selllist"))
                        {
                            NdShopList slist = new NdShopList
                                               {
                                                   Id = short.Parse(selllist.Attribute("id").Value)
                                               };

                            XElement itemElement = selllist.Element("item");
                            if (itemElement != null)
                            {
                                string items = itemElement.Value;
                                items = items.Replace("\n", "").Replace(" ", "");

                                foreach (string i in items.Split(','))
                                {
                                    ItemTemplate it = itable.GetItem(Convert.ToInt32(i));
                                    if (it != null)
                                        slist.Items.Add(new NDShopItem(it));
                                    else
                                        Log.Error($"NpcData: cant find item to trade {i} on npc {shop.Id}");
                                }
                            }

                            shop.Lists.Add(slist.Id, slist);
                        }

                        Shops.Add(shop.Id, shop);
                    }
            }

            Log.Info("NpcData: loaded " + Shops.Count + " merchants.");
            //CLogger.info("NpcData: loaded " + _mults.Count + " multisell lists.");
        }

        public void Buylist(L2Player player, L2Npc trader, short reply)
        {
            if (!Shops.ContainsKey(trader.Template.NpcId))
            {
                player.SendMessage("you shop was not found");
                player.SendActionFailed();
                return;
            }

            NDShop shop = Shops[trader.Template.NpcId];
            GameServerNetworkPacket pk;
            if (!shop.Lists.ContainsKey(reply))
            {
                reply -= 2; // примерка

                if (!shop.Lists.ContainsKey(reply))
                {
                    player.SendMessage("your shop id was just wrong " + reply);
                    player.SendActionFailed();
                }
                else
                    pk = new ShopPreviewList(player, shop.Lists[reply], reply);
            }
            else
            {
                player.SendPacket(new ExBuySellListBuy(player, shop.Lists[reply], 1.10, 1.0, reply));
                player.SendPacket(new ExBuySellListSell(player));
            }
        }

        public void RequestTeleportList(L2Npc npc, L2Player player, int groupId)
        {
            RequestTeleportList(npc, player, groupId, -1);
        }

        public void RequestTeleportList(L2Npc npc, L2Player player, int groupId, int itemId)
        {
            if (!_teleports.Npcs.ContainsKey(npc.Template.NpcId))
            {
                player.ShowHtmPlain("no teleports available for you", npc);
                player.SendActionFailed();
                return;
            }

            ABTeleportGroup group = _teleports.Npcs[npc.Template.NpcId].Groups[groupId];
            StringBuilder sb = new StringBuilder("&$556;<br><br>");
            foreach (ABTeleportEntry e in group.Teles.Values)
            {
                string cost = "";
                int id = itemId != -1 ? itemId : e.ItemId;
                if (player.Level >= 40)
                    cost = " - " + e.Cost + " &#" + id + ";";

                sb.Append("<a action=\"bypass -h teleport_next?ask=" + groupId + "&reply=" + e.Id + "\" msg=\"811;" + e.Name + "\">" + e.Name + "" + cost + "</a><br1>");
            }

            player.TeleportPayId = itemId;
            player.ShowHtmPlain(sb.ToString(), npc);
        }

        public void RequestTeleport(L2Npc npc, L2Player player, int type, int entryId)
        {
            ABTeleportGroup group;
            try
            {
                group = _teleports.Npcs[npc.Template.NpcId].Groups[type];
            }
            catch
            {
                Log.Error($"ND:RequestTeleport cant find teleport group {type}");
                player.SendActionFailed();
                return;
            }

            ABTeleportEntry e = group.Teles[entryId];

            //if (!player.hasItem(e.itemId, e.cost))
            //{
            //    switch (e.itemId)
            //    {
            //        case 57:
            //            player.sendSystemMessage(SystemMessage.SystemMessageId.YOU_NOT_ENOUGH_ADENA);
            //            break;
            //        case 6651:
            //            player.ShowHtm("fornonoblessitem.htm", npc);
            //            break;

            //        default:
            //            player.sendSystemMessage(SystemMessage.SystemMessageId.NOT_ENOUGH_REQUIRED_ITEMS);
            //            break;
            //    }

            //    player.sendActionFailed();
            //    return;
            //}

            switch (e.ItemId)
            {
                case 57:
                    player.ReduceAdena(e.Cost);
                    break;

                default:
                    player.DestroyItemById(e.ItemId, e.Cost);
                    break;
            }
        }

        internal void Preview(L2Player talker, L2Npc myself, int p)
        {
            throw new NotImplementedException();
        }
    }
}