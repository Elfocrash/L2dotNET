namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExNoticePostSent : GameServerNetworkPacket
    {
        private int anim;

        public ExNoticePostSent(int anim)
        {
            this.anim = anim;
        }

        protected internal override void write()
        {
            writeC(0xfe);
            writeH(0xb4);
            writeD(anim);
        }
    }
}