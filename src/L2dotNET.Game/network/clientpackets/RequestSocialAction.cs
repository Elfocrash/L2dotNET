using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestSocialAction : GameServerNetworkRequest
    {
        private int _actionId;

        public RequestSocialAction(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        public override void read()
        {
            _actionId = readD();
        }

        public override void run()
        {
            L2Player player = getClient().CurrentPlayer;
            if (player == null)
                return;

            if (_actionId < 2 || _actionId > 13)
                return;

            player.broadcastPacket(new SocialAction(player.ObjID, _actionId));
        }
    }
}