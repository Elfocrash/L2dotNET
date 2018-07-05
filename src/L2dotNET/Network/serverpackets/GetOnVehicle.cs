using L2dotNET.Models.Player;

namespace L2dotNET.Network.serverpackets
{
    class GetOnVehicle : GameserverPacket
    {
        private readonly L2Player _player;

        public GetOnVehicle(L2Player player)
        {
            _player = player;
        }

        public override void Write()
        {
            WriteByte(0x5C);
            WriteInt(_player.ObjectId);
            WriteInt(_player.Boat.ObjectId);
            WriteInt(_player.BoatX);
            WriteInt(_player.BoatY);
            WriteInt(_player.BoatZ);
        }
    }
}