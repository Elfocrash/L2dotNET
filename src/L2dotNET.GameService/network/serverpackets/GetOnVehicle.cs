using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class GetOnVehicle : GameServerNetworkPacket
    {
        private readonly L2Player player;

        public GetOnVehicle(L2Player player)
        {
            this.player = player;
        }

        protected internal override void write()
        {
            writeC(0x5C);
            writeD(player.ObjId);
            writeD(player.Boat.ObjId);
            writeD(player.BoatX);
            writeD(player.BoatY);
            writeD(player.BoatZ);
        }
    }
}