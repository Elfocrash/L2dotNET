using System;
using System.Threading.Tasks;
using L2dotNET.Models.Player;

namespace L2dotNET.Network.clientpackets.PartyAPI
{
    class RequestPartyLootModification : PacketBase
    {
        private readonly GameClient _client;
        private readonly byte _mode;

        public RequestPartyLootModification(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            packet.MoveOffset(2);
            _client = client;
            _mode = packet.ReadByte();
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

                if ((_mode < player.Party.ItemLooter) || (_mode > player.Party.ItemOrderSpoil) || (_mode == player.Party.ItemDistribution) || (player.Party.Leader.ObjectId != player.ObjectId))
                {
                    player.SendActionFailedAsync();
                    return;
                }

                player.Party.VoteForLootChange(_mode);
            });
        }
    }
}