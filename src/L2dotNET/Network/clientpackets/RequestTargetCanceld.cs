using L2dotNET.model.player;

namespace L2dotNET.Network.clientpackets
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