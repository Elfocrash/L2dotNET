namespace L2dotNET.GameService.Network.Serverpackets
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