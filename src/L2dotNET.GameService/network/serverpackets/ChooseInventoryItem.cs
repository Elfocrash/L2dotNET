using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
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