using L2dotNET.Game.model.vehicles;

namespace L2dotNET.Game.network.l2send
{
    class VehicleDeparture : GameServerNetworkPacket
    {
        private L2Boat boat;
        private int speed;
        private int rotationSpd;

        public VehicleDeparture(L2Boat boat, int speed, int rotationSpd)
        {
            this.boat = boat;
            this.speed = speed;
            this.rotationSpd = rotationSpd;
        }

        protected internal override void write()
        {
            writeC(0x6c);
            writeD(boat.ObjID);
            writeD(speed);
            writeD(rotationSpd);
            writeD(boat.destx);
            writeD(boat.desty);
            writeD(boat.destz);
        }
    }
}
