using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestSocialAction : GameServerNetworkRequest
    {
        private int _actionId;

        public RequestSocialAction(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        public override void Read()
        {
            _actionId = ReadD();
        }

        public override void Run()
        {
            L2Player player = GetClient().CurrentPlayer;
            if (player == null)
                return;

            if ((_actionId < 2) || (_actionId > 13))
                return;

            player.BroadcastPacket(new SocialAction(player.ObjId, _actionId));
        }
    }
}