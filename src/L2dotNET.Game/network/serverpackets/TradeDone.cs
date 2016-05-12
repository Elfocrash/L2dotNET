
namespace L2dotNET.GameService.network.l2send
{
    class TradeDone : GameServerNetworkPacket
    {
        private bool done;
        public TradeDone(bool done = true)
        {
            this.done = done;
        }

        protected internal override void write()
        {
            writeC(0x1c);
            writeD(done ? 1 : 0);
        }
    }
}
