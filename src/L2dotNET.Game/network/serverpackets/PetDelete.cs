namespace L2dotNET.GameService.Network.Serverpackets
{
    class PetDelete : GameServerNetworkPacket
    {
        private readonly byte ObjectSummonType;
        private readonly int ObjID;

        public PetDelete(byte ObjectSummonType, int ObjID)
        {
            this.ObjectSummonType = ObjectSummonType;
            this.ObjID = ObjID;
        }

        protected internal override void write()
        {
            writeC(0xb6);
            writeD(ObjectSummonType);
            writeD(ObjID);
        }
    }
}