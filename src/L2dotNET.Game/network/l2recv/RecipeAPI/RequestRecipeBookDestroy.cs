using L2dotNET.Game.tables;

namespace L2dotNET.Game.network.l2recv
{
    class RequestRecipeBookDestroy : GameServerNetworkRequest
    {
        public RequestRecipeBookDestroy(GameClient client, byte[] data)
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

            player.unregisterRecipe(rec, true);
        }
    }
}
