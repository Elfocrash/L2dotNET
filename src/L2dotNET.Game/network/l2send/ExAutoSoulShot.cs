
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
            writeC(0xFE);
            writeH(0x12);
            writeD(itemId);
            writeD(type);
        }
    }
}
