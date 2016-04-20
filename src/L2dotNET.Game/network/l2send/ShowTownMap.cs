
namespace L2dotNET.Game.network.l2send
{
    public class ShowTownMap : GameServerNetworkPacket
    {
        private string texture;
        private int x;
        private int y;

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
