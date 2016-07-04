using L2dotNET.GameService.Model.Vehicles;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class VehicleInfo : GameServerNetworkPacket
    {
        private readonly L2Boat _boat;

        public VehicleInfo(L2Boat boat)
        {
            this._boat = boat;
        }

        protected internal override void Write()
        {
            WriteC(0x59);
            WriteD(_boat.ObjId);
            WriteD(_boat.X);
            WriteD(_boat.Y);
            WriteD(_boat.Z);
            WriteD(_boat.Heading);
        }
    }
}