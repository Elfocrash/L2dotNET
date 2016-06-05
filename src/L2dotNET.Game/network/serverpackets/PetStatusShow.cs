namespace L2dotNET.GameService.network.serverpackets
{
    class PetStatusShow : GameServerNetworkPacket
    {
        private readonly byte ObjectSummonType;

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