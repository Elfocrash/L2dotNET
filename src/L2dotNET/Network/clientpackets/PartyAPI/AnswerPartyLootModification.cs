using L2dotNET.Models.Player;

namespace L2dotNET.Network.clientpackets.PartyAPI
{
    class AnswerPartyLootModification : PacketBase
    {
        private readonly GameClient _client;
        private readonly byte _answer;

        public AnswerPartyLootModification(Packet packet, GameClient client)
        {
            packet.MoveOffset(2);
            _client = client;
            _answer = packet.ReadByte();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            if (player.Party == null)
            {
                player.SendActionFailed();
                return;
            }

            player.Party.AnswerLootVote(player, _answer);
        }
    }
}