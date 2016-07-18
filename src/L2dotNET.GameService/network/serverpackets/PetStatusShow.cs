namespace L2dotNET.GameService.Network.Serverpackets
{
    class PetStatusShow : GameserverPacket
    {
        private readonly byte _objectSummonType;

        public PetStatusShow(byte objectSummonType)
        {
            _objectSummonType = objectSummonType;
        }

        protected internal override void Write()
        {
            WriteByte(0xb1);
            WriteInt(_objectSummonType);
        }
    }
}