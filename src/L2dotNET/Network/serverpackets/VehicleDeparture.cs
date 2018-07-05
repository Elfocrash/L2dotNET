using L2dotNET.Models.Vehicles;

namespace L2dotNET.Network.serverpackets
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

        public override void Write()
        {
            WriteByte(0x5A);
            WriteInt(_boat.ObjectId);
            WriteInt(_speed);
            WriteInt(_rotationSpd);
            WriteInt(_boat.DestX);
            WriteInt(_boat.DestY);
            WriteInt(_boat.DestZ);
        }
    }
}