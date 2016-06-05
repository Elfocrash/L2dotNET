using L2dotNET.GameService.Model.player;
using L2dotNET.GameService.network.serverpackets;

namespace L2dotNET.GameService.network.clientpackets.VehicleAPI
{
    class RequestGetOffVehicle : GameServerNetworkRequest
    {
        private int boatId;
        private int x;
        private int y;
        private int z;

        public RequestGetOffVehicle(GameClient client, byte[] data)
        {
            base.makeme(client, data);
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

            if (player.Boat == null || player.Boat.ObjID != boatId)
            {
                player.sendActionFailed();
                return;
            }

            if (player.Boat.OnRoute)
            {
                player.sendActionFailed();
                return;
            }

            player.broadcastPacket(new StopMoveInVehicle(player, x, y, z));
            player.broadcastPacket(new GetOffVehicle(player, x, y, z));
            player.Boat = null;
        }
    }
}