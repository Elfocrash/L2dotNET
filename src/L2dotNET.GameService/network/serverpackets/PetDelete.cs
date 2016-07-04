namespace L2dotNET.GameService.Network.Serverpackets
{
    class PetDelete : GameServerNetworkPacket
    {
        private readonly byte _objectSummonType;
        private readonly int _objId;

        public PetDelete(byte objectSummonType, int objId)
        {
            this._objectSummonType = objectSummonType;
            this._objId = objId;
        }

        protected internal override void Write()
        {
            WriteC(0xb6);
            WriteD(_objectSummonType);
            WriteD(_objId);
        }
    }
}