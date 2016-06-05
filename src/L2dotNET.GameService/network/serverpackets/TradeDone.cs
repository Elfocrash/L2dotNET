namespace L2dotNET.GameService.Network.Serverpackets
{
    class TradeDone : GameServerNetworkPacket
    {
        private readonly bool done;

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