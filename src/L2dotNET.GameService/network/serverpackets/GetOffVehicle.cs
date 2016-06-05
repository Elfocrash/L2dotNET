using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class GetOffVehicle : GameServerNetworkPacket
    {
        private readonly L2Player player;
        private readonly int x;
        private readonly int y;
        private readonly int z;

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