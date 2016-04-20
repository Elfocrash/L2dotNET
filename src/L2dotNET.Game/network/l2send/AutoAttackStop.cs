
namespace L2dotNET.Game.network.l2send
{
    class AutoAttackStop : GameServerNetworkPacket
    {
        private int sId;
        public AutoAttackStop(int sId)
        {
            this.sId = sId;
        }

        protected internal override void write()
        {
            writeC(0x2c);
            writeD(sId);
        }
    }
}
