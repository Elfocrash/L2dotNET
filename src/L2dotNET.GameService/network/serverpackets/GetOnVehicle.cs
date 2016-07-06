using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class GetOnVehicle : GameServerNetworkPacket
    {
        private readonly L2Player _player;

        public GetOnVehicle(L2Player player)
        {
            _player = player;
        }

        protected internal override void Write()
        {
            WriteC(0x5C);
            WriteD(_player.ObjId);
            WriteD(_player.Boat.ObjId);
            WriteD(_player.BoatX);
            WriteD(_player.BoatY);
            WriteD(_player.BoatZ);
        }
    }
}