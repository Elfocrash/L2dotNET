using L2dotNET.Game.network.l2send;

namespace L2dotNET.Game.network.l2recv
{
    class StartRotating : GameServerNetworkRequest
    {
        private int degree;
        private int side;
        public StartRotating(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        public override void read()
        {
            degree = readD();
            side = readD();
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            if (player.Airship != null && player.Airship.CaptainId == player.ObjID)
                player.Airship.broadcastPacket(new StartRotation(player.Airship.ObjID, degree, side, player.Airship.RotationSpeed));
            else
                player.broadcastPacket(new StartRotation(player.ObjID, degree, side, 0));
        }
    }
}
