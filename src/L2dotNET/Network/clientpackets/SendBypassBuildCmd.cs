using System;
using System.Threading.Tasks;
using L2dotNET.Handlers;
using L2dotNET.Models.Player;
using Microsoft.Extensions.DependencyInjection;

namespace L2dotNET.Network.clientpackets
{
    class SendBypassBuildCmd : PacketBase
    {
        private readonly GameClient _client;
        private readonly string _alias;
        private readonly IAdminCommandHandler _adminCommandHandler;

        public SendBypassBuildCmd(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _adminCommandHandler = serviceProvider.GetService<IAdminCommandHandler>();
            _alias = packet.ReadString().Trim();
        }

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                var player = _client.CurrentPlayer;
                _adminCommandHandler.Request(player, _alias);
            });
        }
    }
}