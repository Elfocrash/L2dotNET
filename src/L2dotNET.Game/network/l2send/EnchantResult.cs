
namespace L2dotNET.Game.network.l2send
{
    class EnchantResult : GameServerNetworkPacket
    {
        private EnchantResultVal result;
        private int crystal;
        private long count;
        public EnchantResult(EnchantResultVal result, int crystal = 0, long count = 0)
        {
            this.result = result;
            this.crystal = crystal;
            this.count = count;
        }

        protected internal override void write()
        {
            writeC(0x81);
            writeD((int)result);
            //writeD(crystal);
            //writeQ(count);
        }
    }

    public enum EnchantResultVal
    {
        success = 0,
        breakToCount = 1,
        closeWindow = 2,
        breakToOne = 3,
        breakToNothing = 4,
        safeBreak = 5
    }
}
