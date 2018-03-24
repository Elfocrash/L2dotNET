using L2dotNET.Models.player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets
{
    class StartRotating : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _degree;
        private readonly int _side;

        public StartRotating(Packet packet, GameClient client)
        {
            _client = client;
            _degree = packet.ReadInt();
            _side = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            player.BroadcastPacket(new StartRotation(player.ObjId, _degree, _side, 0));
        }
    }
}