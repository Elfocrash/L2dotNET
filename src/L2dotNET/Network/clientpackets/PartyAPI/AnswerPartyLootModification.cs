using System;
using System.Threading.Tasks;
using L2dotNET.Models.Player;

namespace L2dotNET.Network.clientpackets.PartyAPI
{
    class AnswerPartyLootModification : PacketBase
    {
        private readonly GameClient _client;
        private readonly byte _answer;

        public AnswerPartyLootModification(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            packet.MoveOffset(2);
            _client = client;
            _answer = packet.ReadByte();
        }

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                L2Player player = _client.CurrentPlayer;

                if (player.Party == null)
                {
                    player.SendActionFailedAsync();
                    return;
                }

                player.Party.AnswerLootVote(player, _answer);
            });
        }
    }
}