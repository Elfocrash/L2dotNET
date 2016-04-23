
namespace L2dotNET.Game.network.l2send
{
    class ExGetOnAirShip : GameServerNetworkPacket
    {
        private L2Player player;
        public ExGetOnAirShip(L2Player player)
        {
            this.player = player;
        }

        protected internal override void write()
        {
            writeC(0xfe);
            writeH(0x63);

            writeD(player.ObjID);
            writeD(player.Airship.ObjID);
            writeD(player.BoatX);
            writeD(player.BoatY);
            writeD(player.BoatZ);
        }
    }
}
