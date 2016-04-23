
namespace L2dotNET.Game.network.l2send
{
    class NetPing : GameServerNetworkPacket
    {
        private int request;
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
