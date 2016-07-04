using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestTargetCanceld : GameServerNetworkRequest
    {
        public RequestTargetCanceld(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        private short _unselect;

        public override void read()
        {
            _unselect = readH(); //0 esc key, 1 - mouse
        }

        public override void run()
        {
            L2Player player = getClient().CurrentPlayer;

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