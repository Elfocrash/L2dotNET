using L2dotNET.model.player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets
{
    class FinishRotating : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _degree;

        public FinishRotating(Packet packet, GameClient client)
        {
            _client = client;
            _degree = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            player.BroadcastPacket(new StopRotation(player.ObjId, _degree, 0));
        }
    }
}