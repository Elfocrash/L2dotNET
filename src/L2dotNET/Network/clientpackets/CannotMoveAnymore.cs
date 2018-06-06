using System;
using System.Threading.Tasks;
using L2dotNET.Models.Player;

namespace L2dotNET.Network.clientpackets
{
    class CannotMoveAnymore : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _x;
        private readonly int _y;
        private readonly int _z;
        private readonly int _heading;

        public CannotMoveAnymore(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _x = packet.ReadInt();
            _y = packet.ReadInt();
            _z = packet.ReadInt();
            _heading = packet.ReadInt();
        }

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                L2Player player = _client.CurrentPlayer;

                player.NotifyStopMoveAsync(true, true);
            });
        }
    }
}