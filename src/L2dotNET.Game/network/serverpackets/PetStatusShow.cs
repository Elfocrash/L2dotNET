
namespace L2dotNET.GameService.network.l2send
{
    class PetStatusShow : GameServerNetworkPacket
    {
        private byte ObjectSummonType;
        public PetStatusShow(byte ObjectSummonType)
        {
            this.ObjectSummonType = ObjectSummonType;
        }

        protected internal override void write()
        {
            writeC(0xb1);
            writeD(ObjectSummonType);
        }
    }
}
