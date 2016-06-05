using L2dotNET.GameService.Model.player;

namespace L2dotNET.GameService.network.clientpackets.PartyAPI
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