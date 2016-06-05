using L2dotNET.GameService.Model.player;
using L2dotNET.GameService.network.serverpackets;

namespace L2dotNET.GameService.network.clientpackets
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