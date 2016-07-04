using log4net;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Vehicles;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets.VehicleAPI
{
    class RequestMoveToLocationInVehicle : GameServerNetworkRequest
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RequestMoveToLocationInVehicle));
        private int _boatId;
        private int _dx;
        private int _dy;
        private int _dz;
        private int _x;
        private int _y;
        private int _z;

        public RequestMoveToLocationInVehicle(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        public override void Read()
        {
            _boatId = ReadD(); //objectId of boat
            _dx = ReadD();
            _dy = ReadD();
            _dz = ReadD();
            _x = ReadD();
            _y = ReadD();
            _z = ReadD();
        }

        public override void Run()
        {
            L2Player player = Client.CurrentPlayer;

            //You do not possess the correct ticket to board the boat.

            if (player.Summon != null)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.ReleasePetOnBoat);
                player.SendActionFailed();
                return;
            }

            L2Boat boat = null;
            if (player.Boat != null)
                if (player.Boat.ObjId == _boatId)
                    boat = player.Boat;
                else
                {
                    player.SendActionFailed();
                    return;
                }
            else if (player.KnownObjects.ContainsKey(_boatId))
                boat = (L2Boat)player.KnownObjects[_boatId];

            if (boat == null)
            {
                Log.Error($"User requested null boat {_boatId}");
                player.SendActionFailed();
                return;
            }

            if (player.Boat == null)
                player.Boat = boat;

            player.BoatX = _dx;
            player.BoatY = _dy;
            player.BoatZ = _dz;
            player.BroadcastPacket(new MoveToLocationInVehicle(player, _x, _y, _z));
        }
    }
}