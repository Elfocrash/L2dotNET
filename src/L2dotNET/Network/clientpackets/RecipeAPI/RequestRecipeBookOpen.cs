using L2dotNET.Models.Player;

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
        }
    }
}