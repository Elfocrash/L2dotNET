namespace L2dotNET.GameService.Network.Serverpackets
{
    class TradeDone : GameserverPacket
    {
        private readonly bool _done;

        public TradeDone(bool done = true)
        {
            _done = done;
        }

        protected internal override void Write()
        {
            WriteByte(0x1c);
            WriteInt(_done ? 1 : 0);
        }
    }
}