using L2dotNET.Game.logger;
using L2dotNET.Game.model.vehicles;
using L2dotNET.Game.network.l2send;

namespace L2dotNET.Game.network.l2recv
{
    class MoveToLocationInAirShip : GameServerNetworkRequest
    {
        private int shipId;
        private int dx;
        private int dy;
        private int dz;
        private int x;
        private int y;
        private int z;
        public MoveToLocationInAirShip(GameClient client, byte[] data)
        {
            base.makeme(client, data, 2);
        }

        public override void read()
        {
            shipId = readD(); 
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

            L2Airship ship = null;
            if (player.Airship != null)
            {
                if (player.Airship.ObjID == shipId)
                    ship = player.Airship;
                else
                {
                    player.sendActionFailed();
                    return;
                }
            }
            else if (player.knownObjects.ContainsKey(shipId))
                ship = (L2Airship)player.knownObjects[shipId];

            if (ship == null)
            {
                CLogger.error("user requested null airship " + shipId);
                player.sendActionFailed();
                return;
            }

            if (player.Airship == null)
                player.Airship = ship;

            player.BoatX = dx;
            player.BoatY = dy;
            player.BoatZ = dz;
            player.broadcastPacket(new ExMoveToLocationInAirShip(player, dx, dy, dz));
        }
    }
}
