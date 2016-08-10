namespace L2dotNET.Network.serverpackets
{
    class ExNoticePostSent : GameserverPacket
    {
        private readonly int _anim;

        public ExNoticePostSent(int anim)
        {
            _anim = anim;
        }

        public override void Write()
        {
            WriteByte(0xfe);
            WriteShort(0xb4);
            WriteInt(_anim);
        }
    }
}