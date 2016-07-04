using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestUseItem : GameServerNetworkRequest
    {
        public RequestUseItem(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        private int _sId;

        public override void Read()
        {
            _sId = ReadD();
        }

        public override void Run()
        {
            L2Player player = GetClient().CurrentPlayer;

            if (player.PBlockAct == 1)
            {
                player.SendActionFailed();
                return;
            }

            L2Item item = player.GetItemByObjId(_sId);

            if (item == null)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.IncorrectItem);
                return;
            }

            if (player.TradeState != 0)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.CannotPickupOrUseItemWhileTrading);
                player.SendActionFailed();
                return;
            }

            switch (item.Template.Type)
            {
                case ItemTemplate.L2ItemType.Armor:
                case ItemTemplate.L2ItemType.Weapon:
                case ItemTemplate.L2ItemType.Accessary:
                {
                    if (item.IsEquipped == 0)
                    {
                        if (!item.Template.CanEquipChaotic(player.PkKills))
                        {
                            player.SendSystemMessage(SystemMessage.SystemMessageId.YouAreUnableToEquipThisItemWhenYourPkCountIsGreaterThanOrEqualToOne);
                            player.SendActionFailed();
                            return;
                        }

                        if (!item.Template.CanEquipHeroic(player.Heroic) || !item.Template.CanEquipNobless(player.Noblesse) || !item.Template.CanEquipSex(player.Sex))
                        {
                            player.SendSystemMessage(SystemMessage.SystemMessageId.CannotEquipItemDueToBadCondition);
                            player.SendActionFailed();
                            return;
                        }
                    }

                    //int pdollId = player.Inventory.getPaperdollId(item.Template);
                    //player.setPaperdoll(pdollId, item._isEquipped == 1 ? null : item, true);
                    player.BroadcastUserInfo();
                }

                    break;
            }

            if (ItemHandler.Instance.Process(player, item))
                return;

            switch (item.Template.DefaultAction)
            {
                case "action_capsule":
                    Capsule.Instance.Process(player, item);
                    break;
                case "action_call_skill":
                {
                    Skill skill = item.Template.ItemSkill;
                    if (skill != null)
                        player.AddEffect(player, skill, true, false);
                    else
                        player.SendMessage("skill onCall was not found.");
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