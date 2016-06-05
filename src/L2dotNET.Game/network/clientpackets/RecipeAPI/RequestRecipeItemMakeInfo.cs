using L2dotNET.GameService.Model.player;
using L2dotNET.GameService.network.serverpackets;
using L2dotNET.GameService.tables;

namespace L2dotNET.GameService.network.clientpackets.RecipeAPI
{
    class RequestRecipeItemMakeInfo : GameServerNetworkRequest
    {
        public RequestRecipeItemMakeInfo(GameClient client, byte[] data)
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

            player.sendPacket(new RecipeItemMakeInfo(player, rec, 2));
        }
    }
}