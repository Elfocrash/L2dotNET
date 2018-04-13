using L2dotNET.Models.Player;

namespace L2dotNET.Network.clientpackets
{
    class RequestQuestAbort : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _questId;

        public RequestQuestAbort(Packet packet, GameClient client)
        {
            _client = client;
            _questId = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;
            
        }
    }
}