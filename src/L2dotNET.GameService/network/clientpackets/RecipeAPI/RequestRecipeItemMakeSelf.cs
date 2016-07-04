using System;
using System.Linq;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.Tables;

namespace L2dotNET.GameService.Network.Clientpackets.RecipeAPI
{
    class RequestRecipeItemMakeSelf : GameServerNetworkRequest
    {
        public RequestRecipeItemMakeSelf(GameClient client, byte[] data)
        {
            makeme(client, data);
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
                player.SendSystemMessage(SystemMessage.SystemMessageId.RECIPE_INCORRECT);
                player.SendActionFailed();
                return;
            }

            L2Recipe rec = player._recipeBook.FirstOrDefault(r => r.RecipeID == _id);

            if (rec == null)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.RECIPE_INCORRECT);
                player.SendActionFailed();
                return;
            }

            if (player.CurMp < rec._mp_consume)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.NOT_ENOUGH_MP);
                player.SendActionFailed();
                return;
            }

            bool next;

            if (rec._iscommonrecipe == 0)
                next = player.p_create_item >= rec._level;
            else
                next = player.p_create_common_item >= rec._level;

            if (!next)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.CREATE_LVL_TOO_LOW_TO_REGISTER);
                player.SendActionFailed();
                return;
            }

            player.CurMp -= rec._mp_consume;
            StatusUpdate su = new StatusUpdate(player.ObjId);
            su.add(StatusUpdate.CUR_MP, (int)player.CurMp);
            player.SendPacket(su);

            foreach (recipe_item_entry material in rec._materials)
                player.DestroyItemById(material.item.ItemID, material.count);

            if (rec._success_rate < 100)
                if (new Random().Next(0, 100) > rec._success_rate)
                {
                    player.SendPacket(new RecipeItemMakeInfo(player, rec, 0));
                    player.SendActionFailed();
                    return;
                }

            foreach (recipe_item_entry prod in rec._products)
                player.AddItem(prod.item.ItemID, prod.count);

            player.SendPacket(new RecipeItemMakeInfo(player, rec, 1));
        }
    }
}