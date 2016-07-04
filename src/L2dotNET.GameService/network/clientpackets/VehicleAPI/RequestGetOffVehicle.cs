using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets.VehicleAPI
{
    class RequestGetOffVehicle : GameServerNetworkRequest
    {
        private int _boatId;
        private int _x;
        private int _y;
        private int _z;

        public RequestGetOffVehicle(GameClient client, byte[] data)
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