using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class MoveBackwardToLocation : GameServerNetworkRequest
    {
        public MoveBackwardToLocation(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        private int _targetX;
        private int _targetY;
        private int _targetZ;
        private int _originX;
        private int _originY;
        private int _originZ;
        private int _moveMovement;

        public override void Read()
        {
            _targetX = ReadD();
            _targetY = ReadD();
            _targetZ = ReadD();
            _originX = ReadD();
            _originY = ReadD();
            _originZ = ReadD();
            try
            {
                _moveMovement = ReadD(); // is 0 if cursor keys are used  1 if mouse is used
            }
            catch
            {
                // ignore for now
            }
        }

        public override void Run()
        {
            L2Player player = GetClient().CurrentPlayer;

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

            if ((dx * dx + dy * dy) > 98010000) // 9900*9900
            {
                player.SendActionFailed();
                return;
            }

            player.AiCharacter.StopAutoAttack();
            player.MoveTo(_targetX, _targetY, _targetZ);
        }
    }
}