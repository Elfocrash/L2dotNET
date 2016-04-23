
namespace L2dotNET.Game.network.l2send
{
    class ChairSit : GameServerNetworkPacket
    {
        private int sId;
        private int staticId;

        public ChairSit(int sId, int staticId)
        {
            this.sId = sId;
            this.staticId = staticId;
        }

        protected internal override void write()
        {
            writeC(0xe1);
            writeD(sId);
            writeD(staticId);
        }
    }
}
