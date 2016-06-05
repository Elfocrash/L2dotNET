
namespace L2dotNET.GameService.network.l2send
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
            writeD(player.ObjID);
            writeD(player.Boat.ObjID);
            writeD(player.BoatX);
            writeD(player.BoatY);
            writeD(player.BoatZ);
        }
    }
}
