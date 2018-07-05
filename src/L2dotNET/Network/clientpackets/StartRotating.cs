using System;
using System.Threading.Tasks;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets
{
    class StartRotating : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _degree;
        private readonly int _side;

        public StartRotating(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _degree = packet.ReadInt();
            _side = packet.ReadInt();
        }

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                L2Player player = _client.CurrentPlayer;

                player.BroadcastPacketAsync(new StartRotation(player.CharacterId, _degree, _side, 0));
            });
        }
    }
}