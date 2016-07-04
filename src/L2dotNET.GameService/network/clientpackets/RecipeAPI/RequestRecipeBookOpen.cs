using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets.RecipeAPI
{
    class RequestRecipeBookOpen : GameServerNetworkRequest
    {
        public RequestRecipeBookOpen(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        private int _type;

        public override void Read()
        {
            _type = ReadD();
        }

        public override void Run()
        {
            L2Player player = Client.CurrentPlayer;

            player.SendPacket(new RecipeBookItemList(player, _type));
        }
    }
}