namespace L2dotNET.Network.serverpackets
{
    class ExAutoSoulShot : GameserverPacket
    {
        private readonly int _itemId;
        private readonly int _type;

        public ExAutoSoulShot(int itemId, int type)
        {
            _itemId = itemId;
            _type = type;
        }

        public override void Write()
        {
            WriteByte(0xFE);
            WriteShort(0x12);
            WriteInt(_itemId);
            WriteInt(_type);
        }
    }
}