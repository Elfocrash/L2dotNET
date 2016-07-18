using L2dotNET.GameService.Model.Player;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestTargetCanceld : PacketBase
    {
        private readonly GameClient _client;
        private readonly short _unselect;

        public RequestTargetCanceld(Packet packet, GameClient client)
        {
            _client = client;
            _unselect = packet.ReadShort(); //0 esc key, 1 - mouse
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            if ((_unselect == 0) && player.IsCastingNow())
            {
                player.AbortCast();
                return;
            }

            if (player.CurrentTarget != null)
                player.ChangeTarget();
        }
    }
}