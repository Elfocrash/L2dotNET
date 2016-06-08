using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class StartRotating : GameServerNetworkRequest
    {
        private int degree;
        private int side;

        public StartRotating(GameClient client, byte[] data)
        {
            makeme(client, data);
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