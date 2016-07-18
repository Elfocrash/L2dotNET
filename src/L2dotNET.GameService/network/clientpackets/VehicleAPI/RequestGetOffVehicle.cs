using L2dotNET.GameService.Config;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets.VehicleAPI
{
    class RequestGetOffVehicle : PacketBase
    {
        private int _boatId;
        private int _x;
        private int _y;
        private int _z;
        private readonly GameClient _client;

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