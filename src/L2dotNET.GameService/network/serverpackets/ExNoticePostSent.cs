namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExNoticePostSent : GameserverPacket
    {
        private readonly int _anim;

        public ExNoticePostSent(int anim)
        {
            _anim = anim;
        }

        protected internal override void Write()
        {
            WriteByte(0xfe);
            WriteShort(0xb4);
            WriteInt(_anim);
        }
    }
}