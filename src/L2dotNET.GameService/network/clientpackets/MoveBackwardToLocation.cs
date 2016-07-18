using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class MoveBackwardToLocation : PacketBase
    {
        private GameClient _client;

        private int _targetX;
        private int _targetY;
        private int _targetZ;
        private int _originX;
        private int _originY;
        private int _originZ;
        private int _moveMovement;

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

            // player.sendMessage("can see: " + GeoData.getInstance().canSeeCoord(player, _targetX, _targetY, _targetZ, true));

            player.Obsx = -1;

            double dx = _targetX - _originX;
            double dy = _targetY - _originY;

            if (((dx * dx) + (dy * dy)) > 98010000) // 9900*9900
            {
                player.SendActionFailed();
                return;
            }

            player.AiCharacter.StopAutoAttack();
            player.MoveTo(_targetX, _targetY, _targetZ);
        }
    }
}