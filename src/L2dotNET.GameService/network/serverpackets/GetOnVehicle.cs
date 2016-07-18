using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class GetOnVehicle : GameserverPacket
    {
        private readonly L2Player _player;

        public GetOnVehicle(L2Player player)
        {
            _player = player;
        }

        protected internal override void Write()
        {
            WriteByte(0x5C);
            WriteInt(_player.ObjId);
            WriteInt(_player.Boat.ObjId);
            WriteInt(_player.BoatX);
            WriteInt(_player.BoatY);
            WriteInt(_player.BoatZ);
        }
    }
}