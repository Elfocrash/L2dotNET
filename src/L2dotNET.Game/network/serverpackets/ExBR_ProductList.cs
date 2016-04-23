using System.Collections.Generic;
using L2dotNET.Game.tables;

namespace L2dotNET.Game.network.l2send
{
    class ExBR_ProductList : GameServerNetworkPacket
    {
        private IList<CashShopItem> items;
        public ExBR_ProductList(IList<CashShopItem> iList)
        {
            this.items = iList;
        }

        protected internal override void write()
        {
            writeC(0xFE);
            writeH(0xCA);
            writeD(items.Count);

            foreach (CashShopItem item in items)
            {
                writeD(item.Template.ClientID);
                writeH(item.Template.Category);
                writeD(item.Price);
                writeD(item.IsEventItem);
                writeD(item.StartSale);
                writeD(item.EndSale);
                writeC(127); 
                writeC(item.StartHour);
                writeC(item.StartMin);
                writeC(item.EndHour);
                writeC(item.EndMin);
                writeD(item.Stock); // сумма = всего-сколько уже продали
                writeD(item.MaxStock);
            }
        }
    }
}
