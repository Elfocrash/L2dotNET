using System.Linq;
using L2dotNET.model.player;
using L2dotNET.Network.serverpackets;
using L2dotNET.tables;

namespace L2dotNET.Network.clientpackets.RecipeAPI
{
    class RequestRecipeBookDestroy : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _id;

        public RequestRecipeBookDestroy(Packet packet, GameClient client)
        {
            _client = client;
            _id = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            if (player.RecipeBook == null)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.RecipeIncorrect);
                player.SendActionFailed();
                return;
            }

            L2Recipe rec = player.RecipeBook.FirstOrDefault(r => r.RecipeId == _id);

            if (rec == null)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.RecipeIncorrect);
                player.SendActionFailed();
                return;
            }

            player.UnregisterRecipe(rec, true);
        }
    }
}