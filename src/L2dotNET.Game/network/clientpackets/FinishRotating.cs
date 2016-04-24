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

            player.broadcastPacket(new StopRotation(player.ObjID, degree, 0));
        }
    }
}
