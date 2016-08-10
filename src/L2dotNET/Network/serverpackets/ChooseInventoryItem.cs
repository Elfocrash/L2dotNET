namespace L2dotNET.Network.serverpackets
{
    class ChooseInventoryItem : GameserverPacket
    {
        private readonly int _itemId;

        public ChooseInventoryItem(int itemId)
        {
            _itemId = itemId;
        }

        public override void Write()
        {
            WriteByte(0x6f);
            WriteInt(_itemId);
        }
    }
}