using System.Linq;
using L2dotNET.Models.player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Models.vehicles
{
    public class L2Boat : L2Object
    {
        public bool OnRoute = false;

        public L2Boat(int objectId) : base(objectId)
        {
        }

        public override void BroadcastUserInfo()
        {
            foreach (L2Player obj in KnownObjects.Values.OfType<L2Player>())
                obj.SendPacket(new VehicleInfo(this));
        }
    }
}