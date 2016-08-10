namespace L2dotNET.Network.serverpackets
{
    class PetStatusShow : GameserverPacket
    {
        private readonly byte _objectSummonType;

        public PetStatusShow(byte objectSummonType)
        {
            _objectSummonType = objectSummonType;
        }

        public override void Write()
        {
            WriteByte(0xb1);
            WriteInt(_objectSummonType);
        }
    }
}