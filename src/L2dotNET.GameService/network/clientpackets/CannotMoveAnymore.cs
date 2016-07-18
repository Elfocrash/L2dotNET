using L2dotNET.GameService.Config;
using L2dotNET.GameService.Model.Player;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class CannotMoveAnymore : PacketBase
    {
        private int _x;
        private int _y;
        private int _z;
        private int _heading;
        private readonly GameClient _client;

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