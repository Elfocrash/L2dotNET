using System.Collections.Generic;
using L2dotNET.Game.model.items;
using L2dotNET.Game.network.l2send;

namespace L2dotNET.Game.managers
{
    class TradeManager
    {
        private static TradeManager instance = new TradeManager();
        public static TradeManager getInstance()
        {
            return instance;
        }

        public TradeDone trade_success = new TradeDone();
        public TradeDone trade_fail = new TradeDone(false);
        public SystemMessage trade_ok = new SystemMessage(123);//Your trade was successful.

        private bool validateList(L2Player player)
        {
            if (player.currentTrade != null)
            {
                SortedList<int, long> tm = new SortedList<int, long>();
                foreach (int id in player.currentTrade.Keys)
                {
                    L2Item item = player.Inventory.getByObject(id);
                    if (item == null)
                        return false;

                    long num = player.currentTrade[id];

                    if (!item.Template.isStackable() && num != 1)
                        tm.Add(id, 1);

                    if (item.Count < num)
                        tm.Add(id, item.Count);
                }

                if (tm.Count > 0)
                {
                    lock(player.currentTrade)
                        foreach(int key in tm.Keys)
                            player.currentTrade[key] = tm[key];

                    tm.Clear();
                }
            }

            return true;
        }

        public void PersonalTrade(L2Player pl1, L2Player pl2)
        {
            if (!validateList(pl1))
            {
                StopTrade(pl1, pl2, pl1.Name);
                return;
            }

            if (!validateList(pl2))
            {
                StopTrade(pl1, pl2, pl2.Name);
                return;
            }

            List<long[]> list = new List<long[]>();
            if (pl1.currentTrade != null)
            {
                foreach (int id in pl1.currentTrade.Keys)
                    list.Add(new long[] { id, pl1.currentTrade[id] });

                pl2.Inventory.transferHere(pl1, list, false);
                pl1.currentTrade.Clear();
            }

            if (pl2.currentTrade != null)
            {
                list.Clear();

                foreach (int id in pl2.currentTrade.Keys)
                    list.Add(new long[] { id, pl2.currentTrade[id] });

                pl1.Inventory.transferHere(pl2, list, false);
                pl2.currentTrade.Clear();
            }

            pl1.sendPacket(trade_ok);
            pl1.sendPacket(trade_success);
            pl1.sendItemList(true);
            pl1.TradeState = 0;

            pl2.sendPacket(trade_ok);
            pl2.sendPacket(trade_success);
            pl2.sendItemList(true);
            pl2.TradeState = 0;
        }

        private void StopTrade(L2Player pl1, L2Player pl2, string name)
        {
            pl1.TradeState = 0;
            pl1.currentTrade.Clear();
            pl1.sendPacket(trade_fail);
            pl1.sendPacket(new SystemMessage(124).addPlayerName(name));//$c1 has cancelled the trade.
            pl1.requester = null;

            pl2.TradeState = 0;
            pl2.currentTrade.Clear();
            pl2.sendPacket(trade_fail);
            pl2.sendPacket(new SystemMessage(124).addPlayerName(name));//$c1 has cancelled the trade.
            pl2.requester = null;
        }
    }
}
