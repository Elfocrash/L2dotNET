using L2dotNET.GameService.Model.Vehicles;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class VehicleInfo : GameserverPacket
    {
        private readonly L2Boat _boat;

        public VehicleInfo(L2Boat boat)
        {
            _boat = boat;
        }

        public override void Write()
        {
            WriteByte(0x59);
            WriteInt(_boat.ObjId);
            WriteInt(_boat.X);
            WriteInt(_boat.Y);
            WriteInt(_boat.Z);
            WriteInt(_boat.Heading);
        }
    }
}