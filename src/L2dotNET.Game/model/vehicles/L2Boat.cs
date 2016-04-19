using L2dotNET.Game.network.l2send;
using L2dotNET.Game.world;

namespace L2dotNET.Game.model.vehicles
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
