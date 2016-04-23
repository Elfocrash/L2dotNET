using L2dotNET.Game.network.l2send;
using L2dotNET.Game.managers;

namespace L2dotNET.Game.network.l2recv
{
    class RequestBR_MinigameLoadScores : GameServerNetworkRequest
    {
        public RequestBR_MinigameLoadScores(GameClient client, byte[] data)
        {
            base.makeme(client, data, 2);
        }

        public override void read()
        {
            // nothing
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            MinigameRankManager.getInstance().showRank(player);
        }
    }
}
