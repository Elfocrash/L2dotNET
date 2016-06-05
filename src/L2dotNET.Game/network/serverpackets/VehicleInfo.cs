using L2dotNET.GameService.Model.vehicles;

namespace L2dotNET.GameService.network.serverpackets
{
    class VehicleInfo : GameServerNetworkPacket
    {
        private readonly L2Boat boat;

        public VehicleInfo(L2Boat boat)
        {
            this.boat = boat;
        }

        protected internal override void write()
        {
            writeC(0x59);
            writeD(boat.ObjID);
            writeD(boat.X);
            writeD(boat.Y);
            writeD(boat.Z);
            writeD(boat.Heading);
        }
    }
}