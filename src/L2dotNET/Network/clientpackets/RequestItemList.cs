using L2dotNET.Models.Player;

namespace L2dotNET.Network.clientpackets
{
    class RequestItemList : PacketBase
    {
        private readonly GameClient _client;

        public RequestItemList(Packet packet, GameClient client)
        {
            _client = client;
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;
            player.SendItemList(true);
        }
    }
}