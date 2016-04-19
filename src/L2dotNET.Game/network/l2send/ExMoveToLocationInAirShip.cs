
namespace L2dotNET.Game.network.l2send
{
    class ExMoveToLocationInAirShip : GameServerNetworkPacket
    {
        private L2Player player;
        private int x;
        private int y;
        private int z;
        public ExMoveToLocationInAirShip(L2Player player, int x, int y, int z)
        {
            this.player = player;
            this.x = x;
            this.y = y;
            this.z = z;
        }

        protected internal override void write()
        {
            writeC(0xfe);
            writeH(0x6D);
            writeD(player.ObjID);
            writeD(player.Airship.ObjID);
            writeD(player.BoatX);
            writeD(player.BoatY);
            writeD(player.BoatZ);
            writeD(player.Heading);
        }
    }
}
