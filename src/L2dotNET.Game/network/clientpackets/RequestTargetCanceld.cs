namespace L2dotNET.GameService.network.l2recv
{
    class RequestTargetCanceld : GameServerNetworkRequest
    {
        public RequestTargetCanceld(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        private short _unselect;

        public override void read()
        {
            _unselect = readH(); //0 esc key, 1 - mouse
        }

        public override void run()
        {
            L2Player player = getClient().CurrentPlayer;

            if (_unselect == 0 && player.isCastingNow())
            {
                player.abortCast();
                return;
            }

            if (player.CurrentTarget != null)
                player.ChangeTarget();
        }
    }
}