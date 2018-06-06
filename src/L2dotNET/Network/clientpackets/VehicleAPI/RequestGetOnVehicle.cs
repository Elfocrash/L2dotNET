using System;
using System.Threading.Tasks;
using L2dotNET.Models.Player;
using L2dotNET.Models.Vehicles;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets.VehicleAPI
{
    class RequestGetOnVehicle : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _boatId;
        private readonly int _x;
        private readonly int _y;
        private readonly int _z;

        public RequestGetOnVehicle(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
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

                if (player.Boat != null)
                {
                    player.SendActionFailedAsync();
                    return;
                }
            
                player.BoatX = _x;
                player.BoatY = _y;
                player.BoatZ = _z;

                if (player.KnownObjects.ContainsKey(_boatId))
                    player.Boat = (L2Boat)player.KnownObjects[_boatId];

                player.BroadcastPacketAsync(new GetOnVehicle(player));
            });
        }
    }
}