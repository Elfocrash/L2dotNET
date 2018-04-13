using L2dotNET.Models.Player;

namespace L2dotNET.Network.clientpackets.PetAPI
{
    class RequestGetItemFromPet : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _objectId;
        private int _count;
        private readonly int _equipped;

        public RequestGetItemFromPet(Packet packet, GameClient client)
        {
            _client = client;
            _objectId = packet.ReadInt();
            _count = packet.ReadInt();
            if (_count < 0)
                _count = 1;
            _equipped = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;
        }
    }
}