namespace L2dotNET.GameService.Network.Serverpackets
{
    class TradeDone : GameServerNetworkPacket
    {
        private readonly bool _done;

        public TradeDone(bool done = true)
        {
            _done = done;
        }

        protected internal override void Write()
        {
            WriteC(0x1c);
            WriteD(_done ? 1 : 0);
        }
    }
}