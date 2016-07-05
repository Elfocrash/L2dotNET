using System.Collections.Generic;
using System.Linq;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Managers
{
    class TradeManager
    {
        private static readonly TradeManager Instance = new TradeManager();

        public static TradeManager GetInstance()
        {
            return Instance;
        }

        public TradeDone TradeSuccess = new TradeDone();
        public TradeDone TradeFail = new TradeDone(false);
        public SystemMessage TradeOk = new SystemMessage(SystemMessage.SystemMessageId.TradeSuccessful);

        private bool ValidateList(L2Player player)
        {
            if (player.CurrentTrade != null)
            {
                SortedList<int, long> tm = new SortedList<int, long>();
                foreach (int id in player.CurrentTrade.Keys)
                {
                    L2Item item = player.Inventory.GetItemByItemId(id);
                    if (item == null)
                    {
                        return false;
                    }

                    long num = player.CurrentTrade[id];

                    if (!item.Template.Stackable && (num != 1))
                    {
                        tm.Add(id, 1);
                    }

                    if (item.Count < num)
                    {
                        tm.Add(id, item.Count);
                    }
                }

                if (tm.Count > 0)
                {
                    lock (player.CurrentTrade)
                        foreach (int key in tm.Keys)
                        {
                            player.CurrentTrade[key] = tm[key];
                        }

                    tm.Clear();
                }
            }

            return true;
        }

        public void PersonalTrade(L2Player pl1, L2Player pl2)
        {
            if (!ValidateList(pl1))
            {
                StopTrade(pl1, pl2, pl1.Name);
                return;
            }

            if (!ValidateList(pl2))
            {
                StopTrade(pl1, pl2, pl2.Name);
                return;
            }

            List<long[]> list = new List<long[]>();
            if (pl1.CurrentTrade != null)
            {
                list.AddRange(pl1.CurrentTrade.Keys.Select(id => new[] { id, pl1.CurrentTrade[id] }));

                //pl2.Inventory.transferHere(pl1, list, false);
                pl1.CurrentTrade.Clear();
            }

            if (pl2.CurrentTrade != null)
            {
                list.Clear();

                list.AddRange(pl2.CurrentTrade.Keys.Select(id => new[] { id, pl2.CurrentTrade[id] }));

                //pl1.Inventory.transferHere(pl2, list, false);
                pl2.CurrentTrade.Clear();
            }

            pl1.SendPacket(TradeOk);
            pl1.SendPacket(TradeSuccess);
            pl1.SendItemList(true);
            pl1.TradeState = 0;

            pl2.SendPacket(TradeOk);
            pl2.SendPacket(TradeSuccess);
            pl2.SendItemList(true);
            pl2.TradeState = 0;
        }

        private void StopTrade(L2Player pl1, L2Player pl2, string name)
        {
            pl1.TradeState = 0;
            pl1.CurrentTrade.Clear();
            pl1.SendPacket(TradeFail);
            pl1.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.S1CanceledTrade).AddPlayerName(name));
            pl1.Requester = null;

            pl2.TradeState = 0;
            pl2.CurrentTrade.Clear();
            pl2.SendPacket(TradeFail);
            pl2.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.S1CanceledTrade).AddPlayerName(name));
            pl2.Requester = null;
        }
    }
}