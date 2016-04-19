
namespace L2dotNET.Game.network.l2send
{
    class ExAutoSoulShot : GameServerNetworkPacket
    {
        private int itemId;
        private int type;
        public ExAutoSoulShot(int itemId, int type)
        {
            this.itemId = itemId;
            this.type = type;
        }

        protected internal override void write()
        {
            writeC(0xfe);
            writeH(0x0c);
            writeD(itemId);
            writeD(type);
        }
    }
}
