using L2dotNET.model.player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets.RecipeAPI
{
    class RequestRecipeBookOpen : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _type;

        public RequestRecipeBookOpen(Packet packet, GameClient client)
        {
            _client = client;
            _type = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            player.SendPacket(new RecipeBookItemList(player, _type));
        }
    }
}