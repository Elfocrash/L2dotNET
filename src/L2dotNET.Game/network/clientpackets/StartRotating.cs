using L2dotNET.GameService.Model.player;
using L2dotNET.GameService.network.serverpackets;

namespace L2dotNET.GameService.network.clientpackets
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

            player.broadcastPacket(new StartRotation(player.ObjID, degree, side, 0));
        }
    }
}