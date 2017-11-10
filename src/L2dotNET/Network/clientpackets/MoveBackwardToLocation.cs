using L2dotNET.model.player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets
{
    class MoveBackwardToLocation : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _targetX;
        private readonly int _targetY;
        private readonly int _targetZ;
        private readonly int _originX;
        private readonly int _originY;
        private readonly int _originZ;
        private readonly int _moveMovement;

        public MoveBackwardToLocation(Packet packet, GameClient client)
        {
            _client = client;
            _targetX = packet.ReadInt();
            _targetY = packet.ReadInt();
            _targetZ = packet.ReadInt();
            _originX = packet.ReadInt();
            _originY = packet.ReadInt();
            _originZ = packet.ReadInt();
            try
            {
                _moveMovement = packet.ReadInt(); // is 0 if cursor keys are used  1 if mouse is used
            }
            catch
            {
                // ignore for now
            }
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            if (player.IsSittingInProgress() || player.IsSitting())
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.CantMoveSitting);
                player.SendActionFailed();
                return;
            }

            if (player.Boat != null)
            {
                player.SendMessage("cant leave boat.");
                player.SendActionFailed();
                return;
            }

            if (player.CantMove())
            {
                player.SendActionFailed();
                return;
            }

            // player.sendMessage($"can see: {GeoData.getInstance().canSeeCoord(player, _targetX, _targetY, _targetZ, true)}");

            player.Obsx = -1;

            double dx = _targetX - _originX;
            double dy = _targetY - _originY;

            if (((dx * dx) + (dy * dy)) > 98010000) // 9900*9900
            {
                player.SendActionFailed();
                return;
            }

            //player.AiCharacter.StopAutoAttack();
            player.MoveTo(_targetX, _targetY, _targetZ);
        }
    }
}