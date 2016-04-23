using L2dotNET.Game.model.vehicles;

namespace L2dotNET.Game.network.l2send
{
    class ExAirShipInfo : GameServerNetworkPacket
    {
        private L2Airship ship;
        public ExAirShipInfo(L2Airship ship)
        {
            this.ship = ship;
        }

        protected internal override void write()
        {
            writeC(0xfe);
            writeH(0x60);

            writeD(ship.ObjID);
            writeD(ship.X);
            writeD(ship.Y);
            writeD(ship.Z);
            writeD(ship.Heading);

            writeD(ship.CaptainId);
            writeD(ship.Speed);
            writeD(ship.RotationSpeed);
            writeD(ship.HelmId);

            writeD(ship.ControllerX);
            writeD(ship.ControllerY);
            writeD(ship.ControllerZ);
            writeD(ship.CaptainX);
            writeD(ship.CaptainY);
            writeD(ship.CaptainZ);

            writeD(ship.Fuel);
            writeD(ship.MaxFuel);
        }
    }
}
