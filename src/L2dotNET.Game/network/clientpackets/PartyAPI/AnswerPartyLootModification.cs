
namespace L2dotNET.GameService.network.l2recv
{
    class AnswerPartyLootModification : GameServerNetworkRequest
    {
        private byte answer;
        public AnswerPartyLootModification(GameClient client, byte[] data)
        {
            base.makeme(client, data, 2);
        }

        public override void read()
        {
            answer = (byte)readD();
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            if (player.Party == null)
            {
                player.sendActionFailed();
                return;
            }

            player.Party.AnswerLootVote(player, answer);
        }
    }
}
