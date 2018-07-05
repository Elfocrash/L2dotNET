using System;
using System.Threading.Tasks;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets.VehicleAPI
{
    class RequestGetOffVehicle : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _boatId;
        private readonly int _x;
        private readonly int _y;
        private readonly int _z;

        public RequestGetOffVehicle(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _boatId = packet.ReadInt();
            _x = packet.ReadInt();
            _y = packet.ReadInt();
            _z = packet.ReadInt();
        }

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                L2Player player = _client.CurrentPlayer;

                if ((player.Boat == null) || (player.Boat.CharacterId != _boatId))
                {
                    player.SendActionFailedAsync();
                    return;
                }

                if (player.Boat.OnRoute)
                {
                    player.SendActionFailedAsync();
                    return;
                }

                player.BroadcastPacketAsync(new StopMoveInVehicle(player, _x, _y, _z));
                player.BroadcastPacketAsync(new GetOffVehicle(player, _x, _y, _z));
                player.Boat = null;
            });
        }
    }
}