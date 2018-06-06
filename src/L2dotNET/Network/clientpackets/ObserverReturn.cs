using System;
using System.Threading.Tasks;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets
{
    class ObserverReturn : PacketBase
    {
        private readonly GameClient _client;

        public ObserverReturn(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
        }

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                L2Player player = _client.CurrentPlayer;

                player.SendPacketAsync(new ObservationReturn(player.Obsx, player.Obsy, player.Obsz));
            });
        }
    }
}