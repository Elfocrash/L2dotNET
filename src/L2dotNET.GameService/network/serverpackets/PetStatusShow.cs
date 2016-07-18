using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
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