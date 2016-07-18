using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestShowMiniMap : PacketBase
    {
        private readonly GameClient _client;

        public RequestShowMiniMap(Packet packet, GameClient client)
        {
            _client = client;
        }

        public override void RunImpl()
        {
            _client.SendPacket(new ShowMiniMap());
        }
    }
}