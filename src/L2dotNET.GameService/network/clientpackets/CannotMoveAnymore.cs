using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class CannotMoveAnymore : GameServerNetworkRequest
    {
        private int _x;
        private int _y;
        private int _z;
        private int _heading;

        public CannotMoveAnymore(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        public override void Read()
        {
            _x = ReadD();
            _y = ReadD();
            _z = ReadD();
            _heading = ReadD();
        }

        public override void Run()
        {
            L2Player player = Client.CurrentPlayer;

            player.NotifyStopMove(true, true);
        }
    }
}