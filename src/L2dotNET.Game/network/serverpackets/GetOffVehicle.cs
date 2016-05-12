
namespace L2dotNET.GameService.network.l2send
{
    class GetOffVehicle : GameServerNetworkPacket
    {
        private L2Player player;
        private int x;
        private int y;
        private int z;
        public GetOffVehicle(L2Player player, int x, int y, int z)
        {
            this.player = player;
            this.x = x;
            this.y = y;
            this.z = z;
        }

        protected internal override void write()
        {
            writeC(0x5D);
            writeD(player.ObjID);
            writeD(player.Boat.ObjID);
            writeD(x);
            writeD(y);
            writeD(z);
        }
    }
}
