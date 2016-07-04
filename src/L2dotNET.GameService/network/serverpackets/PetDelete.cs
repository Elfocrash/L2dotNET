namespace L2dotNET.GameService.Network.Serverpackets
{
    class PetDelete : GameServerNetworkPacket
    {
        private readonly byte ObjectSummonType;
        private readonly int ObjID;

        public PetDelete(byte ObjectSummonType, int objId)
        {
            this.ObjectSummonType = ObjectSummonType;
            this.ObjID = objId;
        }

        protected internal override void write()
        {
            writeC(0xb6);
            writeD(ObjectSummonType);
            writeD(ObjID);
        }
    }
}