
namespace L2dotNET.Game.network.l2send
{
    class MoveToLocationInVehicle : GameServerNetworkPacket
    {
        private L2Player player;
        private int x;
        private int y;
        private int z;
        public MoveToLocationInVehicle(L2Player player, int x, int y, int z)
        {
            this.player = player;
            this.x = x;
            this.y = y;
            this.z = z;
        }

        protected internal override void write()
        {
            writeC(0x71);

            writeD(player.ObjID);
            writeD(player.Boat.ObjID);
            writeD(player.BoatX);
            writeD(player.BoatY);
            writeD(player.BoatZ);
            writeD(x);
            writeD(y);
            writeD(z);
        }
    }
}
