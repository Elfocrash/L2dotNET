namespace L2dotNET.GameService.Network.Serverpackets
{
    class ChooseInventoryItem : GameserverPacket
    {
        private readonly int _itemId;

        public ChooseInventoryItem(int itemId)
        {
            _itemId = itemId;
        }

        protected internal override void Write()
        {
            WriteByte(0x6f);
            WriteInt(_itemId);
        }
    }
}