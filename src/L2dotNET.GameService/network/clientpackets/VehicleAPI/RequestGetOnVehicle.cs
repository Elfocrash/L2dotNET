using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Vehicles;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets.VehicleAPI
{
    class RequestGetOnVehicle : GameServerNetworkRequest
    {
        private int boatId;
        private int x;
        private int y;
        private int z;

        public RequestGetOnVehicle(GameClient client, byte[] data)
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

            if (player.Boat != null)
            {
                player.sendActionFailed();
                return;
            }

            if (player.Summon != null)
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.RELEASE_PET_ON_BOAT);
                player.sendActionFailed();
                return;
            }

            player.BoatX = x;
            player.BoatY = y;
            player.BoatZ = z;

            if (player.knownObjects.ContainsKey(boatId))
                player.Boat = (L2Boat)player.knownObjects[boatId];

            player.broadcastPacket(new GetOnVehicle(player));
        }
    }
}