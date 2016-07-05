using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestTargetCanceld : GameServerNetworkRequest
    {
        public RequestTargetCanceld(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        private short _unselect;

        public override void Read()
        {
            _unselect = ReadH(); //0 esc key, 1 - mouse
        }

        public override void Run()
        {
            L2Player player = GetClient().CurrentPlayer;

            if ((_unselect == 0) && player.IsCastingNow())
            {
                player.AbortCast();
                return;
            }

            if (player.CurrentTarget != null)
            {
                player.ChangeTarget();
            }
        }
    }
}