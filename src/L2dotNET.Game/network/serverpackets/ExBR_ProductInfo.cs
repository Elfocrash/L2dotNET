using L2dotNET.Game.tables;

namespace L2dotNET.Game.network.l2send
{
    class ExBR_ProductInfo : GameServerNetworkPacket
    {
        private CashShopItem item;
        public ExBR_ProductInfo(CashShopItem item)
        {
            this.item = item;
        }

        protected internal override void write()
        {
            writeC(0xFE);
            writeH(0xCB);
            writeD(item.Template.ClientID);
            writeD(item.Price);

            writeD(1);
            writeD(item.Template.Item.ItemID);
            writeD(item.Count);
            writeD(item.Template.Item.Weight);
            writeD(item.Template.Item.is_trade);
        }
    }
}
