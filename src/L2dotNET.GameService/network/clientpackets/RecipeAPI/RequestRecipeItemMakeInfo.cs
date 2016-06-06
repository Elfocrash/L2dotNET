using System.Linq;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.Tables;

namespace L2dotNET.GameService.Network.Clientpackets.RecipeAPI
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

            L2Recipe rec = player._recipeBook.FirstOrDefault(r => r.RecipeID == _id);

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