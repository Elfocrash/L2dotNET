using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Vehicles;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets.VehicleAPI
{
    class RequestGetOnVehicle : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _boatId;
        private readonly int _x;
        private readonly int _y;
        private readonly int _z;

        public RequestGetOnVehicle(Packet packet, GameClient client)
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

            if (player.Boat != null)
            {
                player.SendActionFailed();
                return;
            }

            if (player.Summon != null)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.ReleasePetOnBoat);
                player.SendActionFailed();
                return;
            }

            player.BoatX = _x;
            player.BoatY = _y;
            player.BoatZ = _z;

            if (player.KnownObjects.ContainsKey(_boatId))
                player.Boat = (L2Boat)player.KnownObjects[_boatId];

            player.BroadcastPacket(new GetOnVehicle(player));
        }
    }
}