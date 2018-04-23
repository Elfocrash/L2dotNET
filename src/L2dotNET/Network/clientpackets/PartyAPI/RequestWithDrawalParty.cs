using System;
using L2dotNET.Models.Player;

namespace L2dotNET.Network.clientpackets.PartyAPI
{
    class RequestWithdrawalParty : PacketBase
    {
        private readonly GameClient _client;

        public RequestWithdrawalParty(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            if (player.Party == null)
            {
                player.SendActionFailed();
                return;
            }

            player.Party.Leave(player);
        }
    }
}