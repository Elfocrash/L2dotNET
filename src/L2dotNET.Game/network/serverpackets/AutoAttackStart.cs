
namespace L2dotNET.Game.network.l2send
{
    class AutoAttackStart : GameServerNetworkPacket
    {
        private int sId;
        public AutoAttackStart(int sId)
        {
            this.sId = sId;
        }

        protected internal override void write()
        {
            writeC(0x2b);
            writeD(sId);
        }
    }
}
