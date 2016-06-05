using System;
using L2dotNET.GameService.Model.player;
using L2dotNET.GameService.network.serverpackets;
using L2dotNET.GameService.tables;

namespace L2dotNET.GameService.network.clientpackets.RecipeAPI
{
    class RequestRecipeItemMakeSelf : GameServerNetworkRequest
    {
        public RequestRecipeItemMakeSelf(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        private int _id;

        public override void read()
        {
            _id = readD();
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            if (player._recipeBook == null)
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.RECIPE_INCORRECT);
                player.sendActionFailed();
                return;
            }

            L2Recipe rec = null;

            foreach (L2Recipe r in player._recipeBook)
            {
                if (r.RecipeID == _id)
                {
                    rec = r;
                    break;
                }
            }

            if (rec == null)
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.RECIPE_INCORRECT);
                player.sendActionFailed();
                return;
            }

            if (player.CurMP < rec._mp_consume)
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.NOT_ENOUGH_MP);
                player.sendActionFailed();
                return;
            }

            bool next = true;

            if (rec._iscommonrecipe == 0)
                next = player.p_create_item >= rec._level;
            else
                next = player.p_create_common_item >= rec._level;

            if (!next)
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.CREATE_LVL_TOO_LOW_TO_REGISTER);
                player.sendActionFailed();
                return;
            }

            foreach (recipe_item_entry material in rec._materials)
            {
                long count = player.Inventory.getItemCount(material.item.ItemID);
                if (count < material.count)
                {
                    SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.MISSING_S2_S1_TO_CREATE);
                    sm.AddItemName(material.item.ItemID);
                    sm.AddItemCount(material.count - count);
                    player.sendPacket(sm);
                    player.sendActionFailed();
                    return;
                }
            }

            player.CurMP -= rec._mp_consume;
            StatusUpdate su = new StatusUpdate(player.ObjID);
            su.add(StatusUpdate.CUR_MP, (int)player.CurMP);
            player.sendPacket(su);

            foreach (recipe_item_entry material in rec._materials)
            {
                player.Inventory.destroyItem(material.item.ItemID, material.count, true, true);
            }

            if (rec._success_rate < 100)
            {
                if (new Random().Next(0, 100) > rec._success_rate)
                {
                    player.sendPacket(new RecipeItemMakeInfo(player, rec, 0));
                    player.sendActionFailed();
                    return;
                }
            }

            foreach (recipe_item_entry prod in rec._products)
            {
                // if(prod.rate == 100)
                player.Inventory.addItem(prod.item, prod.count, 0, true, true);
                // else
            }

            player.sendPacket(new RecipeItemMakeInfo(player, rec, 1));
        }
    }
}