namespace L2dotNET.GameService.Network.Serverpackets
{
    class PetStatusShow : GameServerNetworkPacket
    {
        private readonly byte _objectSummonType;

        public PetStatusShow(byte objectSummonType)
        {
            _objectSummonType = objectSummonType;
        }

        protected internal override void Write()
        {
            WriteC(0xb1);
            WriteD(_objectSummonType);
        }
    }
}