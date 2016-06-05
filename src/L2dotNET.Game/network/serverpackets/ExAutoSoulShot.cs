namespace L2dotNET.GameService.Network.Serverpackets
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