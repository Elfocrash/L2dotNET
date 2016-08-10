namespace L2dotNET.Network.serverpackets
{
    class PetDelete : GameserverPacket
    {
        private readonly byte _objectSummonType;
        private readonly int _objId;

        public PetDelete(byte objectSummonType, int objId)
        {
            _objectSummonType = objectSummonType;
            _objId = objId;
        }

        public override void Write()
        {
            WriteByte(0xb6);
            WriteInt(_objectSummonType);
            WriteInt(_objId);
        }
    }
}