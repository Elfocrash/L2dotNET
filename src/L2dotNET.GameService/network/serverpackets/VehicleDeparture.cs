using L2dotNET.GameService.Model.Vehicles;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class VehicleDeparture : GameServerNetworkPacket
    {
        private readonly L2Boat _boat;
        private readonly int _speed;
        private readonly int _rotationSpd;

        public VehicleDeparture(L2Boat boat, int speed, int rotationSpd)
        {
            _boat = boat;
            _speed = speed;
            _rotationSpd = rotationSpd;
        }

        protected internal override void Write()
        {
            WriteC(0x5A);
            WriteD(_boat.ObjId);
            WriteD(_speed);
            WriteD(_rotationSpd);
            WriteD(_boat.DestX);
            WriteD(_boat.DestY);
            WriteD(_boat.DestZ);
        }
    }
}