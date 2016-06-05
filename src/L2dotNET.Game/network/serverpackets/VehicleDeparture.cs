using L2dotNET.GameService.model.vehicles;

namespace L2dotNET.GameService.network.l2send
{
    class VehicleDeparture : GameServerNetworkPacket
    {
        private readonly L2Boat boat;
        private readonly int speed;
        private readonly int rotationSpd;

        public VehicleDeparture(L2Boat boat, int speed, int rotationSpd)
        {
            this.boat = boat;
            this.speed = speed;
            this.rotationSpd = rotationSpd;
        }

        protected internal override void write()
        {
            writeC(0x5A);
            writeD(boat.ObjID);
            writeD(speed);
            writeD(rotationSpd);
            writeD(boat.DestX);
            writeD(boat.DestY);
            writeD(boat.DestZ);
        }
    }
}
