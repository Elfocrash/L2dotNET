using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class FinishRotating : GameServerNetworkRequest
    {
        private int _degree;

        public FinishRotating(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        public override void Read()
        {
            _degree = ReadD();
        }

        public override void Run()
        {
            L2Player player = Client.CurrentPlayer;

            player.BroadcastPacket(new StopRotation(player.ObjId, _degree, 0));
        }
    }
}