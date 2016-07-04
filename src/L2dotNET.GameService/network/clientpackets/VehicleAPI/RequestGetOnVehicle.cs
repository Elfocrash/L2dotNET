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

            if (player.Boat != null)
            {
                player.SendActionFailed();
                return;
            }

            if (player.Summon != null)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.RELEASE_PET_ON_BOAT);
                player.SendActionFailed();
                return;
            }

            player.BoatX = x;
            player.BoatY = y;
            player.BoatZ = z;

            if (player.KnownObjects.ContainsKey(boatId))
                player.Boat = (L2Boat)player.KnownObjects[boatId];

            player.BroadcastPacket(new GetOnVehicle(player));
        }
    }
}