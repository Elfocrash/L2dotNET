using L2dotNET.Models.Player;

namespace L2dotNET.Network.serverpackets
{
    class MoveToLocationInVehicle : GameserverPacket
    {
        private readonly L2Player _player;
        private readonly int _x;
        private readonly int _y;
        private readonly int _z;

        public MoveToLocationInVehicle(L2Player player, int x, int y, int z)
        {
            _player = player;
            _x = x;
            _y = y;
            _z = z;
        }

        public override void Write()
        {
            WriteByte(0x71);

            WriteInt(_player.ObjectId);
            WriteInt(_player.Boat.ObjectId);
            WriteInt(_player.BoatX);
            WriteInt(_player.BoatY);
            WriteInt(_player.BoatZ);
            WriteInt(_x);
            WriteInt(_y);
            WriteInt(_z);
        }
    }
}