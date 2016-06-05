using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets.RecipeAPI
{
    class RequestRecipeBookOpen : GameServerNetworkRequest
    {
        public RequestRecipeBookOpen(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        private int _type;

        public override void read()
        {
            _type = readD();
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            player.sendPacket(new RecipeBookItemList(player, _type));
        }
    }
}