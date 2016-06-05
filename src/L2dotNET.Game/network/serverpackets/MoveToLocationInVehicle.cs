using L2dotNET.GameService.Model.player;

namespace L2dotNET.GameService.network.serverpackets
{
    class MoveToLocationInVehicle : GameServerNetworkPacket
    {
        private readonly L2Player player;
        private readonly int x;
        private readonly int y;
        private readonly int z;

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