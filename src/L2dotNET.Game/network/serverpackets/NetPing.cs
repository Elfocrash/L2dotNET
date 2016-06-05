
namespace L2dotNET.GameService.network.l2send
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
