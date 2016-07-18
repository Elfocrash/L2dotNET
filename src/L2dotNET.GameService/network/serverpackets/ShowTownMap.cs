using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    public class ShowTownMap : GameserverPacket
    {
        private readonly string _texture;
        private readonly int _x;
        private readonly int _y;

        public ShowTownMap(string texture, int x, int y)
        {
            _texture = texture;
            _x = x;
            _y = y;
        }

        public override void Write()
        {
            WriteByte(0xde);
            WriteString(_texture);
            WriteInt(_x);
            WriteInt(_y);
        }
    }
}