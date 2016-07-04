using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class StartRotating : GameServerNetworkRequest
    {
        private int _degree;
        private int _side;

        public StartRotating(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        public override void Read()
        {
            _degree = ReadD();
            _side = ReadD();
        }

        public override void Run()
        {
            L2Player player = Client.CurrentPlayer;

            player.BroadcastPacket(new StartRotation(player.ObjId, _degree, _side, 0));
        }
    }
}