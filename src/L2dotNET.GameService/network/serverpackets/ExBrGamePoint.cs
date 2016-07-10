namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExBrGamePoint : GameServerNetworkPacket
    {
        private readonly int _id;
        private readonly long _points;

        public ExBrGamePoint(int id, long points)
        {
            _id = id;
            _points = points;
        }

        protected internal override void Write()
        {
            WriteC(0xFE);
            WriteH(0xC9);

            WriteD(_id);
            WriteQ(_points);
            WriteD(0);
        }
    }
}