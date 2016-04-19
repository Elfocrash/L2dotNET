using System.Collections.Generic;
using L2dotNET.Game.model.items;
using L2dotNET.Game.model.playable;
using L2dotNET.Game.network.l2send;

namespace L2dotNET.Game.model.inventory
{
    public class InvPet : InvTemplate
    {
        private L2Pet owner;
        public InvPet(L2Pet owner)
        {
            this.owner = owner;
        }

        public void transferHere(L2Player player, List<long[]> items, bool update)
        {
            InventoryUpdate iu = update ? new InventoryUpdate() : null;
            PetInventoryUpdate piu = update ? new PetInventoryUpdate() : null;
            List<int> nulled = new List<int>();
            foreach (L2Item item in player.Inventory.Items.Values)
            {
                foreach (long[] itemd in items)
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
                                nulled.Add((int)itemd[0]);
                                if (ex)
                                {
                                    foreach (L2Item itp in Items.Values)
                                        if (itp.Template.ItemID == item.Template.ItemID)
                                        {
                                            itp.Count += item.Count;
                                            itp.sql_update();
                                            if (update)
                                                piu.addModItem(itp);
                                            break;
                                        }

                                    item.sql_delete();
                                }
                                else
                                {
                                    item.Location = L2Item.L2ItemLocation.pet;
                                    Items.Add(item.ObjID, item);
                                    item.sql_update();

                                    if (update)
                                        piu.addNewItem(item);
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
                                            if (update)
                                                piu.addModItem(itp);
                                            break;
                                        }
                                }
                                else
                                {
                                    L2Item ins = new L2Item(item.Template);
                                    ins.Count = itemd[1];
                                    ins.Location = L2Item.L2ItemLocation.pet;
                                    Items.Add(ins.ObjID, ins);
                                    
                                    ins.sql_insert(player.ObjID);

                                    if (update)
                                        piu.addNewItem(ins);
                                }

                                if (update)
                                    iu.addModItem(item);
                            }
                        }
                        else
                        {
                            nulled.Add((int)itemd[0]);
                            item.Location = L2Item.L2ItemLocation.pet;
                            Items.Add(item.ObjID, item);

                            item.sql_update();

                            if (update)
                            {
                                iu.addDelItem(item);
                                piu.addNewItem(item);
                            }
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

            if (update)
            {
                player.sendPacket(iu);
                player.sendPacket(piu);
            }
        }

        public void dbLoad(L2Item item)
        {
            Items.Add(item.ObjID, item);
        }

        public void transferFrom(L2Player player, List<long[]> transfer, bool update)
        {
            InventoryUpdate iu = update ? new InventoryUpdate() : null;
            PetInventoryUpdate piu = update ? new PetInventoryUpdate() : null;
            List<int> nulled = new List<int>();
            foreach (L2Item item in Items.Values)
            {
                foreach (long[] itemd in transfer)
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
                                nulled.Add((int)itemd[0]);
                                if (ex)
                                {
                                    foreach (L2Item itp in player.getAllItems())
                                        if (itp.Template.ItemID == item.Template.ItemID)
                                        {
                                            itp.Count += item.Count;
                                            itp.sql_update();
                                            if (update)
                                            {
                                                iu.addModItem(itp);
                                                piu.addDelItem(item);
                                            }

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
                                    {
                                        iu.addNewItem(item);
                                        piu.addDelItem(item);
                                    }
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
                                            {
                                                iu.addModItem(itp);
                                                piu.addModItem(item);
                                            }

                                            break;
                                        }
                                }
                                else
                                {
                                    L2Item ins = new L2Item(item.Template);
                                    ins.Count = itemd[1];
                                    ins.Location = L2Item.L2ItemLocation.inventory;
                                    player.Inventory.Items.Add(ins.ObjID, ins);
                                    ins.sql_insert(player.ObjID);

                                    if (update)
                                    {
                                        iu.addNewItem(ins);
                                        piu.addDelItem(item);
                                    }
                                }
                            }
                        }
                        else
                        {
                            nulled.Add((int)itemd[0]);
                            item.Location = L2Item.L2ItemLocation.inventory;
                            player.Inventory.Items.Add(item.ObjID, item);

                            item.sql_update();

                            if (update)
                            {
                                iu.addNewItem(item);
                                piu.addDelItem(item);
                            }
                        }

                        foreach (QuestInfo qi in player._quests)
                            if (!qi.completed)
                                qi._template.onEarnItem(player, qi.stage, item.Template.ItemID);
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

            if (update)
            {
                player.sendPacket(iu);
                player.sendPacket(piu);
            }
        }
    }
}
