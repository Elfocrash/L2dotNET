using L2dotNET.Models.Player;

namespace L2dotNET.Network.serverpackets
{
    class StopMoveInVehicle : GameserverPacket
    {
        private readonly L2Player _player;
        private readonly int _x;
        private readonly int _y;
        private readonly int _z;

        public StopMoveInVehicle(L2Player player, int x, int y, int z)
        {
            _player = player;
            _x = x;
            _y = y;
            _z = z;
        }

        public override void Write()
        {
            WriteByte(0x72);
            WriteInt(_player.CharacterId);
            WriteInt(_player.Boat.CharacterId);
            WriteInt(_x);
            WriteInt(_y);
            WriteInt(_z);
            WriteInt(_player.Heading);
        }
    }
}