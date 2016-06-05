namespace L2dotNET.GameService.network.serverpackets
{
    class Revive : GameServerNetworkPacket
    {
        private readonly int ObjID;

        public Revive(int ObjID)
        {
            this.ObjID = ObjID;
        }

        protected internal override void write()
        {
            writeC(0x07);
            writeD(ObjID);
        }
    }
}