using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class CannotMoveAnymore : GameServerNetworkRequest
    {
        private int x;
        private int y;
        private int z;
        private int heading;

        public CannotMoveAnymore(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        public override void read()
        {
            x = readD();
            y = readD();
            z = readD();
            heading = readD();
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            player.NotifyStopMove(true, true);
        }
    }
}