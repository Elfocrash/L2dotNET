namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExNoticePostSent : GameServerNetworkPacket
    {
        private readonly int _anim;

        public ExNoticePostSent(int anim)
        {
            _anim = anim;
        }

        protected internal override void Write()
        {
            WriteC(0xfe);
            WriteH(0xb4);
            WriteD(_anim);
        }
    }
}