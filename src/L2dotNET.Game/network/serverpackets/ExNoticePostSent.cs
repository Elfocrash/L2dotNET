
namespace L2dotNET.GameService.network.l2send
{
    class ExNoticePostSent : GameServerNetworkPacket
    {
        private int anim;
        public ExNoticePostSent(int anim)
        {
            anim = anim;
        }

        protected internal override void write()
        {
            writeC(0xfe);
            writeH(0xb4);
            writeD(anim);
        }
    }
}
