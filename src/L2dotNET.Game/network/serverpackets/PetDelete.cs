
namespace L2dotNET.GameService.network.l2send
{
    class PetDelete : GameServerNetworkPacket
    {
        private byte ObjectSummonType;
        private int ObjID;
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
