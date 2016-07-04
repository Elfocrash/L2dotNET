using log4net;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Vehicles;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets.VehicleAPI
{
    class RequestMoveToLocationInVehicle : GameServerNetworkRequest
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RequestMoveToLocationInVehicle));
        private int boatId;
        private int dx;
        private int dy;
        private int dz;
        private int x;
        private int y;
        private int z;

        public RequestMoveToLocationInVehicle(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            boatId = readD(); //objectId of boat
            dx = readD();
            dy = readD();
            dz = readD();
            x = readD();
            y = readD();
            z = readD();
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            //You do not possess the correct ticket to board the boat.

            if (player.Summon != null)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.RELEASE_PET_ON_BOAT);
                player.SendActionFailed();
                return;
            }

            L2Boat boat = null;
            if (player.Boat != null)
                if (player.Boat.ObjId == boatId)
                    boat = player.Boat;
                else
                {
                    player.SendActionFailed();
                    return;
                }
            else if (player.KnownObjects.ContainsKey(boatId))
                boat = (L2Boat)player.KnownObjects[boatId];

            if (boat == null)
            {
                log.Error($"User requested null boat {boatId}");
                player.SendActionFailed();
                return;
            }

            if (player.Boat == null)
                player.Boat = boat;

            player.BoatX = dx;
            player.BoatY = dy;
            player.BoatZ = dz;
            player.BroadcastPacket(new MoveToLocationInVehicle(player, x, y, z));
        }
    }
}