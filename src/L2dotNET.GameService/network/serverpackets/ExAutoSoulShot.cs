namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExAutoSoulShot : GameServerNetworkPacket
    {
        private readonly int _itemId;
        private readonly int _type;

        public ExAutoSoulShot(int itemId, int type)
        {
            _itemId = itemId;
            _type = type;
        }

        protected internal override void Write()
        {
            WriteC(0xFE);
            WriteH(0x12);
            WriteD(_itemId);
            WriteD(_type);
        }
    }
}