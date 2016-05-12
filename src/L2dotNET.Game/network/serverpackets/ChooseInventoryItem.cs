
namespace L2dotNET.GameService.network.l2send
{
    class ChooseInventoryItem : GameServerNetworkPacket
    {
        private int itemId;
        public ChooseInventoryItem(int itemId)
        {
            this.itemId = itemId;
        }

        protected internal override void write()
        {
            writeC(0x6f);
            writeD(itemId);
        }
    }
}
