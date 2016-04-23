using L2dotNET.Game.model.vehicles;

namespace L2dotNET.Game.network.l2send
{
    class ExMoveToLocationAirShip : GameServerNetworkPacket
    {
        private L2Airship ship;
        public ExMoveToLocationAirShip(L2Airship ship)
        {
            this.ship = ship;
        }

        protected internal override void write()
        {
            writeC(0xfe);
            writeH(0x65);

            writeD(ship.ObjID);
            writeD(ship.destx);
            writeD(ship.desty);
            writeD(ship.destz);
            writeD(ship.X);
            writeD(ship.Y);
            writeD(ship.Z);
        }
    }
}
