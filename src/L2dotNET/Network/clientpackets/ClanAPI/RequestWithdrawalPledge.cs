using System;
using System.Threading.Tasks;
using L2dotNET.Models.Player;

namespace L2dotNET.Network.clientpackets.ClanAPI
{
    class RequestWithdrawalPledge : PacketBase
    {
        private readonly GameClient _client;

        public RequestWithdrawalPledge(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
        }

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                var player = _client.CurrentPlayer;
            });
        }
    }
}