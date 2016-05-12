using L2dotNET.GameService.model.vehicles;

namespace L2dotNET.GameService.network.l2send
{
    class VehicleInfo : GameServerNetworkPacket
    {
        private L2Boat boat;
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
