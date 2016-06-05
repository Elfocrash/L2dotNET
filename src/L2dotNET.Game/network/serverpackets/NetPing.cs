namespace L2dotNET.GameService.Network.Serverpackets
{
    class NetPing : GameServerNetworkPacket
    {
        private readonly int request;

        public NetPing(int request)
        {
            this.request = request;
        }

        protected internal override void write()
        {
            writeC(0xd9);
            writeD(request);
        }
    }
}