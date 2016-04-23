using L2dotNET.Game.network.l2send;
using L2dotNET.Game.world;
using L2dotNET.Game.tables;

namespace L2dotNET.Game.model.vehicles
{
    class L2Airship : L2Object
    {
        public bool OnRoute = false;
        public int CaptainId;
        public int Speed = 400;
        public int RotationSpeed = 1800;
        public int HelmId = IdFactory.Instance.nextId();
        public int ControllerX = 0x16e;
        public int ControllerY;
        public int ControllerZ = 0x6b;
        public int CaptainX = 0x15c;
        public int CaptainY;
        public int CaptainZ = 0x69;
        public int Fuel = 500;
        public int MaxFuel = 600;
        public override void broadcastUserInfo()
        {
            foreach (L2Object obj in knownObjects.Values)
                if (obj is L2Player)
                    obj.sendPacket(new ExAirShipInfo(this));
        }

        public override void onAction(L2Player player)
        {
            player.sendPacket(new MyTargetSelected(HelmId, 0));
            base.onAction(player);
        }
    }
}
