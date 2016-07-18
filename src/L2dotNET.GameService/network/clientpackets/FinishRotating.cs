using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets
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