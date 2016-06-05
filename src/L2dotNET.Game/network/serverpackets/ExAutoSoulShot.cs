namespace L2dotNET.GameService.network.l2send
{
    class ExAutoSoulShot : GameServerNetworkPacket
    {
        private readonly int itemId;
        private readonly int type;

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