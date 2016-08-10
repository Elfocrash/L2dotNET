using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets
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