using L2dotNET.GameService.Model.player;
using L2dotNET.GameService.network.serverpackets;
using L2dotNET.GameService.world;

namespace L2dotNET.GameService.Model.vehicles
{
    public class L2Boat : L2Object
    {
        public bool OnRoute = false;

        public override void broadcastUserInfo()
        {
            foreach (L2Object obj in knownObjects.Values)
                if (obj is L2Player)
                    obj.sendPacket(new VehicleInfo(this));
        }
    }
}