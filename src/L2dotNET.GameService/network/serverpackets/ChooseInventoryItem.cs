namespace L2dotNET.GameService.Network.Serverpackets
{
    class ChooseInventoryItem : GameServerNetworkPacket
    {
        private readonly int _itemId;

        public ChooseInventoryItem(int itemId)
        {
            this._itemId = itemId;
        }

        protected internal override void Write()
        {
            WriteC(0x6f);
            WriteD(_itemId);
        }
    }
}