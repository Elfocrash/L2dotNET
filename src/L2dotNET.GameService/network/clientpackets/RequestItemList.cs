using L2dotNET.GameService.Config;
using L2dotNET.GameService.Model.Player;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets
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