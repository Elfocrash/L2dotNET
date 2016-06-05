namespace L2dotNET.GameService.network.serverpackets
{
    public class ShowTownMap : GameServerNetworkPacket
    {
        private readonly string texture;
        private readonly int x;
        private readonly int y;

        public ShowTownMap(string texture, int x, int y)
        {
            this.texture = texture;
            this.x = x;
            this.y = y;
        }

        protected internal override void write()
        {
            writeC(0xde);
            writeS(texture);
            writeD(x);
            writeD(y);
        }
    }
}