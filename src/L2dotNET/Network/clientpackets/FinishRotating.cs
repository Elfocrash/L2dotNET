using System;
using System.Threading.Tasks;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets
{
    class FinishRotating : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _degree;

        public FinishRotating(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _degree = packet.ReadInt();
        }

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                L2Player player = _client.CurrentPlayer;

                player.BroadcastPacketAsync(new StopRotation(player.ObjectId, _degree, 0));
            });
        }
    }
}