using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class StopMoveInVehicle : GameServerNetworkPacket
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

        protected internal override void Write()
        {
            WriteC(0x72);
            WriteD(_player.ObjId);
            WriteD(_player.Boat.ObjId);
            WriteD(_x);
            WriteD(_y);
            WriteD(_z);
            WriteD(_player.Heading);
        }
    }
}