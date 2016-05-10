using System;
using L2dotNET.Game.model.items;
using L2dotNET.Game.network.l2send;
using L2dotNET.Game.tables;
using L2dotNET.Game.model.skills2;

namespace L2dotNET.Game.network.l2recv
{
    class RequestUseItem : GameServerNetworkRequest
    {
        public RequestUseItem(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        private int sID;
        public override void read()
        {
            sID = readD();
        }

        public override void run()
        {
            L2Player player = getClient().CurrentPlayer;

            if (player._p_block_act == 1)
            {
                player.sendActionFailed();
                return;
            }

            L2Item item = player.getItemByObjId(sID);

            if (item == null)
            {
                player.sendSystemMessage(352); //Incorrect item.
                return;
            }

            if(player.TradeState != 0)
            {
                player.sendSystemMessage(149); //You cannot pick up or use items while trading.
                player.sendActionFailed();
                return;
            }

            switch (item.Template.Type)
            {
                case ItemTemplate.L2ItemType.armor:
                case ItemTemplate.L2ItemType.weapon:
                case ItemTemplate.L2ItemType.accessary:
                    {
                        if (item._isEquipped == 0)
                        {
                            if (!item.Template.canEquipChaotic(player.PkKills))
                            {
                                //You are unable to equip this item when your PK count is greater than or equal to one.
                                player.sendSystemMessage(1685);
                                player.sendActionFailed();
                                return;
                            }

                            if (!item.Template.canEquipHeroic(player.Heroic) ||
                                !item.Template.canEquipNobless(player.Noblesse) ||
                                !item.Template.canEquipSex(player.Sex))
                            {
                                //You do not meet the required condition to equip that item.
                                player.sendSystemMessage(1518);
                                player.sendActionFailed();
                                return;
                            }
                        }

                        int pdollId = player.Inventory.getPaperdollId(item.Template);
                        player.setPaperdoll(pdollId, item._isEquipped == 1 ? null : item, true);
                        player.broadcastUserInfo();
                    }
                    break;
            }

            if (ItemHandler.Instance.Process(player, item))
                return;

            switch (item.Template.default_action)
            {
                case "action_capsule":
                    Capsule.getInstance().Process(player, item);
                    break;
                case "action_call_skill":
                    {
                        TSkill skill = item.Template.item_skill;
                        if (skill != null)
                            player.addEffect(player, skill, true, false);
                        else
                            player.sendMessage("skill onCall was not found.");
                    }
                    break;
            }

            //switch (item.Template._actionType)
            //{
            //    case ItemTemplate.L2ItemActionType.action_show_html:
            //        player.sendPacket(new NpcHtmlMessage(player, item.Template._htmFile, player.ObjID, item.Template.ItemID));
            //        break;
            //    case ItemTemplate.L2ItemActionType.action_recipe:
            //        {
            //            bool next = false;

            //            int cur0 = player.ItemLimit_RecipeDwarven, cur1 = player.ItemLimit_Warehouse;
            //            if (player._recipeBook != null)
            //            {
            //                foreach (L2Recipe rec in player._recipeBook)
            //                {
            //                    if (rec._item_id == item.Template.ItemID)
            //                    {
            //                        //That recipe is already registered.
            //                        player.sendSystemMessage(840);
            //                        player.sendActionFailed();
            //                        return;
            //                    }

            //                    if(rec._iscommonrecipe == 0)
            //                        cur0--;
            //                    else
            //                        cur1--;
            //                }
            //            }

            //            L2Recipe newr = RecipeTable.getInstance().getByItemId(item.Template.ItemID);

            //            if (newr._iscommonrecipe == 0)
            //                next = player.p_create_item >= newr._level;
            //            else
            //                next = player.p_create_common_item >= newr._level;

            //            if (!next)
            //            {
            //                //Your Create Item level is too low to register this recipe.
            //                player.sendSystemMessage(404);
            //                player.sendActionFailed();
            //                return;
            //            }

            //            next = true;

            //            if (newr._iscommonrecipe == 0)
            //                next = cur0 > 0;
            //            else
            //                next = cur1 > 0;

            //            if (!next)
            //            {
            //                //No further recipes may be registered.
            //                player.sendSystemMessage(841);
            //                player.sendActionFailed();
            //                return;
            //            }

            //            player.registerRecipe(newr, true, false);
            //            player.Inventory.destroyItem(item, 1, true, true);
            //        }
            //        break;
            //}



        }
    }
}
