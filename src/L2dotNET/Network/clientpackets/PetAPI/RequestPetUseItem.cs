using L2dotNET.model.items;
using L2dotNET.model.player;

namespace L2dotNET.Network.clientpackets.PetAPI
{
    class RequestPetUseItem : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _sId;

        public RequestPetUseItem(Packet packet, GameClient client)
        {
            _client = client;
            _sId = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;
        }
    }
}