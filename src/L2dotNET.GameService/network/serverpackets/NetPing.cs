namespace L2dotNET.GameService.Network.Serverpackets
{
    class NetPing : GameserverPacket
    {
        private readonly int _request;

        public NetPing(int request)
        {
            _request = request;
        }

        protected internal override void Write()
        {
            WriteByte(0xd9);
            WriteInt(_request);
        }
    }
}