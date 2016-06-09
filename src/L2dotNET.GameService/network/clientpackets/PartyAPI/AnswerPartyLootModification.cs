using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Clientpackets.PartyAPI
{
    class AnswerPartyLootModification : GameServerNetworkRequest
    {
        private byte answer;

        public AnswerPartyLootModification(GameClient client, byte[] data)
        {
            makeme(client, data, 2);
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