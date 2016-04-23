
namespace L2dotNET.Game.network.l2send
{
    class Revive : GameServerNetworkPacket
    {
        private int ObjID;
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
