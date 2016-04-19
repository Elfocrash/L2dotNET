using L2dotNET.Game.network.l2send;
using System;

namespace L2dotNET.Game.network.l2recv
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
