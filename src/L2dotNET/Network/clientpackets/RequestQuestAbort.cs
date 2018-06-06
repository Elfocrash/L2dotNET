using System;
using System.Threading.Tasks;
using L2dotNET.Models.Player;

namespace L2dotNET.Network.clientpackets
{
    class RequestQuestAbort : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _questId;

        public RequestQuestAbort(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _questId = packet.ReadInt();
        }

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                L2Player player = _client.CurrentPlayer;
            });
        }
    }
}