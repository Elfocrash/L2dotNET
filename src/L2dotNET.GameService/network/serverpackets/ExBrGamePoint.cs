using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExBrGamePoint : GameserverPacket
    {
        private readonly int _id;
        private readonly long _points;

        public ExBrGamePoint(int id, long points)
        {
            _id = id;
            _points = points;
        }

        public override void Write()
        {
            WriteByte(0xFE);
            WriteShort(0xC9);

            WriteInt(_id);
            WriteLong(_points);
            WriteInt(0);
        }
    }
}