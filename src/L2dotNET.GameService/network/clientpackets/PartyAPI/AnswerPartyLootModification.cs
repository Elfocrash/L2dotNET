using L2dotNET.GameService.Model.Player;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets.PartyAPI
{
    class AnswerPartyLootModification : PacketBase
    {
        private readonly GameClient _client;
        private readonly byte _answer;

        public AnswerPartyLootModification(Packet packet, GameClient client)
        {
            packet.MoveOffset(2);
            _client = client;
            _answer = (byte)packet.ReadByte();
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