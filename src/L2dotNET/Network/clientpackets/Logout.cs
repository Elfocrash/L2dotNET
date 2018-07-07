using System;
using System.Threading.Tasks;
using L2dotNET.Models.Player;
using L2dotNET.Network.loginauth;
using L2dotNET.Network.serverpackets;
using Microsoft.Extensions.DependencyInjection;

namespace L2dotNET.Network.clientpackets
{
    class Logout : PacketBase
    {
        private readonly GameClient _client;
        private readonly AuthThread _authThread;
        public Logout(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _authThread = serviceProvider.GetService<AuthThread>();
            _client = client;
        }

        public override async Task RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            if (player == null)
            {
                return;
            }

            if (player.PBlockAct == 1)
            {
                player.SendActionFailedAsync();
                return;
            }

            if (player.isInCombat())
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.CantLogoutWhileFighting);
                player.SendActionFailedAsync();
                return;
            }

            _client.Disconnect();
            player.SendPacketAsync(new LeaveWorld());
        }
    }
}