using L2dotNET.Game.managers;

namespace L2dotNET.Game.network.l2recv
{
    class RequestBR_MinigameInsertScore : GameServerNetworkRequest
    {
        private int m_CurrentScore;
        public RequestBR_MinigameInsertScore(GameClient client, byte[] data)
        {
            base.makeme(client, data, 2);
        }

        public override void read()
        {
            m_CurrentScore = readD();
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            MinigameRankManager.getInstance().insertMe(player, m_CurrentScore);
        }
    }
}
