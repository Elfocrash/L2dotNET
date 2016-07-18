using L2dotNET.GameService.Model.Player;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class CannotMoveAnymore : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _x;
        private readonly int _y;
        private readonly int _z;
        private readonly int _heading;

        public CannotMoveAnymore(Packet packet, GameClient client)
        {
            _client = client;
            _x = packet.ReadInt();
            _y = packet.ReadInt();
            _z = packet.ReadInt();
            _heading = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            player.NotifyStopMove(true, true);
        }
    }
}