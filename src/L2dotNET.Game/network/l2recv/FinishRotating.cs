using L2dotNET.Game.network.l2send;

namespace L2dotNET.Game.network.l2recv
{
    class FinishRotating : GameServerNetworkRequest
    {
        private int degree;
        public FinishRotating(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        public override void read()
        {
            degree = readD();
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            if (player.Airship != null && player.Airship.CaptainId == player.ObjID)
            {
                player.Airship.Heading = degree;
                player.Airship.broadcastPacket(new StopRotation(player.Airship.ObjID, degree, player.Airship.RotationSpeed));
            }
            else
                player.broadcastPacket(new StopRotation(player.ObjID, degree, 0));
        }
    }
}
