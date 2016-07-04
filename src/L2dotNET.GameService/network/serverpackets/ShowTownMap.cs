namespace L2dotNET.GameService.Network.Serverpackets
{
    public class ShowTownMap : GameServerNetworkPacket
    {
        private readonly string _texture;
        private readonly int _x;
        private readonly int _y;

        public ShowTownMap(string texture, int x, int y)
        {
            this._texture = texture;
            this._x = x;
            this._y = y;
        }

        protected internal override void Write()
        {
            WriteC(0xde);
            WriteS(_texture);
            WriteD(_x);
            WriteD(_y);
        }
    }
}