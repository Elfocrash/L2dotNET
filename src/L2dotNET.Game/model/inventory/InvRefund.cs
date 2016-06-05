using System.Collections.Generic;
using L2dotNET.GameService.model.items;
using L2dotNET.GameService.network.l2send;

namespace L2dotNET.GameService.model.inventory
{
    public class InvRefund
    {
        private readonly L2Player _owner;
        public List<L2Item> _items = new List<L2Item>();
        public InvRefund(L2Player owner)
        {
            _owner = owner;
        }

        public void validate()
        {
            if (_items.Count > 12)
            {
                List<L2Item> s2 = new List<L2Item>();
                int nt = _items.Count - 12, xt = 1;

                foreach (L2Item item in _items)
                {
                    if (nt >= xt)
                    {
                        xt++;
                        continue;
                    }

                    s2.Add(item);
                }

                _items.Clear();
                _items = s2;
            }
        }

        public void transferHere(L2Player player, List<long[]> items, bool update)
        {
            InventoryUpdate iu = update ? new InventoryUpdate() : null;
            List<int> nulled = new List<int>();
            foreach (L2Item item in player.Inventory.Items.Values)
            {
                foreach (long[] itemd in items)
                {
                    if (item.ObjID == itemd[0])
                    {
                        if (item.Template.isStackable())
                        {
                            if (itemd[1] >= item.Count)
                            {
                                nulled.Add((int)itemd[0]);

                                item.Location = L2Item.L2ItemLocation.refund;
                                _items.Add(item);
                                item.sql_update();

                                if (update)
                                    iu.addDelItem(item);
                            }
                            else
                            {
                                item.Count -= itemd[1];

                                L2Item ins = new L2Item(item.Template);
                                ins.Count = itemd[1];
                                ins.Location = L2Item.L2ItemLocation.refund;
                                _items.Add(ins);
                                    
                                ins.sql_insert(_owner.ObjID);

                                if (update)
                                    iu.addModItem(item);
                            }
                        }
                        else
                        {
                            nulled.Add((int)itemd[0]);
                            item.Location = L2Item.L2ItemLocation.refund;
                            _items.Add(item);

                            item.sql_update();

                            if (update)
                                iu.addModItem(item);
                        }
                    }
                }
             }

            lock (player.Inventory.Items)
            {
                foreach (int itemd in nulled)
                {
                    player.Inventory.Items.Remove(itemd);
                }
            }
        }
    }
}
