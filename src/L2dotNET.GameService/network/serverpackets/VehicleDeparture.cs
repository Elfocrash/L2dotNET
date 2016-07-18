using L2dotNET.GameService.Model.Vehicles;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class VehicleDeparture : GameserverPacket
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
            WriteByte(0x5A);
            WriteInt(_boat.ObjId);
            WriteInt(_speed);
            WriteInt(_rotationSpd);
            WriteInt(_boat.DestX);
            WriteInt(_boat.DestY);
            WriteInt(_boat.DestZ);
        }
    }
}