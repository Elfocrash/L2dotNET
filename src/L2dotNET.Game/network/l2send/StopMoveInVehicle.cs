
namespace L2dotNET.Game.network.l2send
{
    class StopMoveInVehicle : GameServerNetworkPacket
    {
        private L2Player player;
        private int x;
        private int y;
        private int z;
        public StopMoveInVehicle(L2Player player, int x, int y, int z)
        {
            this.player = player;
            this.x = x;
            this.y = y;
            this.z = z;
        }

        protected internal override void write()
        {
            writeC(0x7f);
            writeD(player.ObjID);
            writeD(player.Boat.ObjID);
            writeD(x);
            writeD(y);
            writeD(z);
            writeD(player.Heading);
        }
    }
}
