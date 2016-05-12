
namespace L2dotNET.GameService.network.l2recv
{
    class MoveBackwardToLocation : GameServerNetworkRequest
    {
        public MoveBackwardToLocation(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        private int _targetX;
        private int _targetY;
        private int _targetZ;
        private int _originX;
        private int _originY;
        private int _originZ;
        private int _moveMovement;

        public override void read()
        {
            _targetX = readD();
            _targetY = readD();
            _targetZ = readD();
            _originX = readD();
            _originY = readD();
            _originZ = readD();
            try
            {
                _moveMovement = readD(); // is 0 if cursor keys are used  1 if mouse is used
            }
            catch
            {
                // ignore for now
            }
        }

        public override void run()
        {
            L2Player player = getClient().CurrentPlayer;

            if (player.isSittingInProgress() || player.isSitting())
            {
                player.sendSystemMessage(31);//You cannot move while sitting.
                player.sendActionFailed();
                return;
            }

            if (player.Boat != null)
            {
                player.sendMessage("cant leave boat.");
                player.sendActionFailed();
                return;
            }

            if (player.cantMove())
            {
                player.sendActionFailed();
                return;
            }

           // player.sendMessage("can see: " + GeoData.getInstance().canSeeCoord(player, _targetX, _targetY, _targetZ, true));

            if (player._obsx != -1)
                player._obsx = -1;

            double dx = _targetX - _originX;
            double dy = _targetY - _originY;

            if ((dx * dx + dy * dy) > 98010000) // 9900*9900
            {
                player.sendActionFailed();
                return;
            }
            player.AICharacter.StopAutoAttack();
            player.MoveTo(_targetX, _targetY, _targetZ);
        }
    }
}
