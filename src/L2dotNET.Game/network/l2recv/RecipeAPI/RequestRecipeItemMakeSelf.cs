using System;
using L2dotNET.Game.network.l2send;
using L2dotNET.Game.tables;

namespace L2dotNET.Game.network.l2recv
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
                //The recipe is incorrect.
                player.sendSystemMessage(852);
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
                //The recipe is incorrect.
                player.sendSystemMessage(852);
                player.sendActionFailed();
                return;
            }

            if (player.CurMP < rec._mp_consume)
            {
                player.sendSystemMessage(24); //Not enough MP.
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
                player.sendSystemMessage(404); //Your Create Item level is too low to register this recipe.
                player.sendActionFailed();
                return;
            }
            
            foreach (recipe_item_entry material in rec._materials)
            {
                long count = player.Inventory.getItemCount(material.item.ItemID);
                if (count < material.count)
                {
                    //You are missing $s2 $s1 required to create that.
                    SystemMessage sm = new SystemMessage(854);
                    sm.addItemName(material.item.ItemID);
                    sm.addItemCount(material.count - count);
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
