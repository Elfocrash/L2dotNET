using System.Collections.Generic;
using System.Linq;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Quests;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Model.Inventory
{
    public class InvPrivateWarehouse : InvTemplate
    {
        private readonly L2Player _owner;

        public InvPrivateWarehouse(L2Player owner)
        {
            _owner = owner;
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
                        bool ex = Items.Values.Any(itp => itp.Template.ItemID == item.Template.ItemID);

                        if (item.Template.isStackable())
                        {
                            if (itemd[1] >= item.Count)
                            {
                                nulled.Add((int)itemd[0]);
                                if (ex)
                                {
                                    foreach (L2Item itp in Items.Values.Where(itp => itp.Template.ItemID == item.Template.ItemID))
                                    {
                                        itp.Count += item.Count;
                                        itp.sql_update();
                                        break;
                                    }

                                    item.sql_delete();
                                }
                                else
                                {
                                    item.Location = L2Item.L2ItemLocation.warehouse;
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
                                    foreach (L2Item itp in Items.Values.Where(itp => itp.Template.ItemID == item.Template.ItemID))
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
                                    ins.Location = L2Item.L2ItemLocation.warehouse;
                                    Items.Add(ins.ObjID, ins);

                                    ins.sql_insert(_owner.ObjID);
                                }

                                if (update)
                                    iu.addModItem(item);
                            }
                        }
                        else
                        {
                            nulled.Add((int)itemd[0]);
                            item.Location = L2Item.L2ItemLocation.warehouse;
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
                        bool ex = player.getAllItems().Any(itp => itp.Template.ItemID == item.Template.ItemID);

                        if (item.Template.isStackable())
                        {
                            if (itemd[1] >= item.Count)
                            {
                                nulled.Add(itemd[0]);
                                if (ex)
                                {
                                    foreach (L2Item itp in player.getAllItems().Where(itp => itp.Template.ItemID == item.Template.ItemID))
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
                                    foreach (L2Item itp in player.getAllItems().Where(itp => itp.Template.ItemID == item.Template.ItemID))
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

                        foreach (QuestInfo qi in _owner._quests.Where(qi => !qi.completed))
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