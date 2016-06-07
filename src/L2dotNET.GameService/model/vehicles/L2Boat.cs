using System.Linq;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Vehicles
{
    public class L2Boat : L2Object
    {
        public bool OnRoute = false;

        public override void broadcastUserInfo()
        {
            foreach (L2Player obj in knownObjects.Values.OfType<L2Player>())
                obj.sendPacket(new VehicleInfo(this));
        }
    }
}