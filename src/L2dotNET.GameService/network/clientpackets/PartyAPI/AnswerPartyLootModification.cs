using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Clientpackets.PartyAPI
{
    class AnswerPartyLootModification : GameServerNetworkRequest
    {
        private byte _answer;

        public AnswerPartyLootModification(GameClient client, byte[] data)
        {
            Makeme(client, data, 2);
        }

        public override void Read()
        {
            _answer = (byte)ReadD();
        }

        public override void Run()
        {
            L2Player player = Client.CurrentPlayer;

            if (player.Party == null)
            {
                player.SendActionFailed();
                return;
            }

            player.Party.AnswerLootVote(player, _answer);
        }
    }
}