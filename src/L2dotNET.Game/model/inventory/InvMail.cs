using System.Collections.Generic;
using L2dotNET.GameService.model.items;
using L2dotNET.GameService.network.l2send;

namespace L2dotNET.GameService.model.inventory
{
    public class InvMail : InvTemplate
    {
        private readonly L2Player _owner;

        public InvMail(L2Player owner)
        {
            _owner = owner;
        }

        public void transferHere(L2Player player, List<int[]> items, bool update)
        {
            InventoryUpdate iu = update ? new InventoryUpdate() : null;
            List<int> nulled = new List<int>();
            foreach (L2Item item in player.Inventory.Items.Values)
            {
                foreach (int[] itemd in items)
                {
                    if (item.ObjID == itemd[0])
                    {
                        bool ex = false;
                        foreach (L2Item itp in Items.Values)
                            if (itp.Template.ItemID == item.Template.ItemID)
                            {
                                ex = true;
                                break;
                            }

                        if (item.Template.isStackable())
                        {
                            if (itemd[1] >= item.Count)
                            {
                                nulled.Add(itemd[0]);
                                if (ex)
                                {
                                    foreach (L2Item itp in Items.Values)
                                        if (itp.Template.ItemID == item.Template.ItemID)
                                        {
                                            itp.Count += item.Count;
                                            itp.sql_update();
                                            break;
                                        }

                                    item.sql_delete();
                                }
                                else
                                {
                                    item.Location = L2Item.L2ItemLocation.mail;
                                    Items.Add(item.ObjID, item);
                                    item.sql_update();
                                }

                                if (update)
                                    iu.addDelItem(item);
                            }
                            else
                            {
                                item.Count -= itemd[1];

                                if (ex)
                                {
                                    foreach (L2Item itp in Items.Values)
                                        if (itp.Template.ItemID == item.Template.ItemID)
                                        {
                                            itp.Count += itemd[1];
                                            itp.sql_update();
                                            break;
                                        }
                                }
                                else
                                {
                                    L2Item ins = new L2Item(item.Template);
                                    ins.Count = itemd[1];
                                    ins.Location = L2Item.L2ItemLocation.mail;
                                    Items.Add(ins.ObjID, ins);

                                    ins.sql_insert(_owner.ObjID);
                                }

                                if (update)
                                    iu.addModItem(item);
                            }
                        }
                        else
                        {
                            nulled.Add(itemd[0]);
                            item.Location = L2Item.L2ItemLocation.mail;
                            Items.Add(item.ObjID, item);

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

        public void dbLoad(L2Item item)
        {
            Items.Add(item.ObjID, item);
        }

        public void transferFrom(L2Player player, List<int[]> transfer, bool update)
        {
            InventoryUpdate iu = update ? new InventoryUpdate() : null;
            List<int> nulled = new List<int>();
            foreach (L2Item item in Items.Values)
            {
                foreach (int[] itemd in transfer)
                {
                    if (item.ObjID == itemd[0])
                    {
                        bool ex = false;
                        foreach (L2Item itp in player.getAllItems())
                            if (itp.Template.ItemID == item.Template.ItemID)
                            {
                                ex = true;
                                break;
                            }

                        if (item.Template.isStackable())
                        {
                            if (itemd[1] >= item.Count)
                            {
                                nulled.Add(itemd[0]);
                                if (ex)
                                {
                                    foreach (L2Item itp in player.getAllItems())
                                        if (itp.Template.ItemID == item.Template.ItemID)
                                        {
                                            itp.Count += item.Count;
                                            itp.sql_update();
                                            if (update)
                                                iu.addModItem(itp);

                                            break;
                                        }

                                    item.sql_delete();
                                }
                                else
                                {
                                    item.Location = L2Item.L2ItemLocation.inventory;
                                    player.Inventory.Items.Add(item.ObjID, item);
                                    item.sql_update();

                                    if (update)
                                        iu.addNewItem(item);
                                }
                            }
                            else
                            {
                                item.Count -= itemd[1];
                                if (ex)
                                {
                                    foreach (L2Item itp in player.getAllItems())
                                        if (itp.Template.ItemID == item.Template.ItemID)
                                        {
                                            itp.Count += itemd[1];
                                            itp.sql_update();
                                            if (update)
                                                iu.addModItem(itp);

                                            break;
                                        }
                                }
                                else
                                {
                                    L2Item ins = new L2Item(item.Template);
                                    ins.Count = itemd[1];
                                    ins.Location = L2Item.L2ItemLocation.inventory;
                                    player.Inventory.Items.Add(ins.ObjID, ins);
                                    ins.sql_insert(_owner.ObjID);

                                    if (update)
                                        iu.addNewItem(ins);
                                }
                            }
                        }
                        else
                        {
                            nulled.Add(itemd[0]);
                            item.Location = L2Item.L2ItemLocation.inventory;
                            player.Inventory.Items.Add(item.ObjID, item);

                            item.sql_update();

                            if (update)
                                iu.addNewItem(item);
                        }

                        foreach (QuestInfo qi in _owner._quests)
                            if (!qi.completed)
                                qi._template.onEarnItem(_owner, qi.stage, item.Template.ItemID);
                    }
                }
            }

            lock (Items)
            {
                foreach (int itemd in nulled)
                {
                    Items.Remove(itemd);
                }
            }
        }
    }
}