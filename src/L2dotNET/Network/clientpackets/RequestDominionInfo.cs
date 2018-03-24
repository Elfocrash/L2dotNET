using L2dotNET.Models.player;

namespace L2dotNET.Network.clientpackets
{
    class RequestDominionInfo : PacketBase
    {
        private readonly GameClient _client;

        public RequestDominionInfo(Packet packet, GameClient client)
        {
            packet.MoveOffset(2);
            _client = client;
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;
        }
    }
}