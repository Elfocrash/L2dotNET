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

        public RequestGetOffVehicle(Packet packet, GameClient client)
        {
            _client = client;
            _boatId = packet.ReadInt();
            _x = packet.ReadInt();
            _y = packet.ReadInt();
            _z = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            if ((player.Boat == null) || (player.Boat.ObjId != _boatId))
            {
                player.SendActionFailed();
                return;
            }

            if (player.Boat.OnRoute)
            {
                player.SendActionFailed();
                return;
            }

            player.BroadcastPacket(new StopMoveInVehicle(player, _x, _y, _z));
            player.BroadcastPacket(new GetOffVehicle(player, _x, _y, _z));
            player.Boat = null;
        }
    }
}