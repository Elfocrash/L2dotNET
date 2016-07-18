using log4net;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Vehicles;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets.VehicleAPI
{
    class RequestMoveToLocationInVehicle : PacketBase
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RequestMoveToLocationInVehicle));
        private int _boatId;
        private int _dx;
        private int _dy;
        private int _dz;
        private int _x;
        private int _y;
        private int _z;
        private GameClient _client;

        public RequestMoveToLocationInVehicle(Packet packet, GameClient client)
        {
            _client = client;
            _boatId = packet.ReadInt(); //objectId of boat
            _dx = packet.ReadInt();
            _dy = packet.ReadInt();
            _dz = packet.ReadInt();
            _x = packet.ReadInt();
            _y = packet.ReadInt();
            _z = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            //You do not possess the correct ticket to board the boat.

            if (player.Summon != null)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.ReleasePetOnBoat);
                player.SendActionFailed();
                return;
            }

            L2Boat boat = null;
            if (player.Boat != null)
            {
                if (player.Boat.ObjId == _boatId)
                {
                    boat = player.Boat;
                }
                else
                {
                    player.SendActionFailed();
                    return;
                }
            }
            else if (player.KnownObjects.ContainsKey(_boatId))
            {
                boat = (L2Boat)player.KnownObjects[_boatId];
            }

            if (boat == null)
            {
                Log.Error($"User requested null boat {_boatId}");
                player.SendActionFailed();
                return;
            }

            if (player.Boat == null)
            {
                player.Boat = boat;
            }

            player.BoatX = _dx;
            player.BoatY = _dy;
            player.BoatZ = _dz;
            player.BroadcastPacket(new MoveToLocationInVehicle(player, _x, _y, _z));
        }
    }
}