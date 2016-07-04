using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Vehicles;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets.VehicleAPI
{
    class RequestGetOnVehicle : GameServerNetworkRequest
    {
        private int _boatId;
        private int _x;
        private int _y;
        private int _z;

        public RequestGetOnVehicle(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        public override void Read()
        {
            _boatId = ReadD();
            _x = ReadD();
            _y = ReadD();
            _z = ReadD();
        }

        public override void Run()
        {
            L2Player player = Client.CurrentPlayer;

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