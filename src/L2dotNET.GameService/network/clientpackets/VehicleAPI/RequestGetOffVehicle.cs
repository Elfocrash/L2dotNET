using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets.VehicleAPI
{
    class RequestGetOffVehicle : GameServerNetworkRequest
    {
        private int boatId;
        private int x;
        private int y;
        private int z;

        public RequestGetOffVehicle(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            boatId = readD();
            x = readD();
            y = readD();
            z = readD();
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            if ((player.Boat == null) || (player.Boat.ObjId != boatId))
            {
                player.SendActionFailed();
                return;
            }

            if (player.Boat.OnRoute)
            {
                player.SendActionFailed();
                return;
            }

            player.BroadcastPacket(new StopMoveInVehicle(player, x, y, z));
            player.BroadcastPacket(new GetOffVehicle(player, x, y, z));
            player.Boat = null;
        }
    }
}