using System.Collections.Generic;
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
            Log.Info($"MultiSell: {Lists.Count} lists");
        }

        public MultiSellList GetList(int listId)
        {
            return Lists.ContainsKey(listId) ? Lists[listId] : null;
        }
    }
}