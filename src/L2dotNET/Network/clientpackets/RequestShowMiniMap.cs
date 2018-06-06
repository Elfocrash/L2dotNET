using System;
using System.Threading.Tasks;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets
{
    class RequestShowMiniMap : PacketBase
    {
        private readonly GameClient _client;

        public RequestShowMiniMap(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
        }

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                _client.SendPacketAsync(new ShowMiniMap());
            });
        }
    }
}