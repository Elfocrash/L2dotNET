using System.Collections.Generic;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class TradeStart : GameServerNetworkPacket
    {
        private L2Player player;
        private readonly List<L2Item> trade = new List<L2Item>();
        private readonly int partnerId;

        public TradeStart(L2Player player)
        {
            this.player = player;
            partnerId = player.requester.ObjID;
            foreach (L2Item item in player.getAllNonQuestItems())
            {
                if ((item.Template.is_trade == 0) || (item.AugmentationID > 0) || (item._isEquipped == 1))
                    continue;

                if (item.Template.Type == ItemTemplate.L2ItemType.asset)
                    continue;

                trade.Add(item);
            }
        }

        protected internal override void write()
        {
            writeC(0x1E);
            writeD(partnerId);
            writeH(trade.Count);

            foreach (L2Item item in trade)
            {
                writeH(item.Template.Type1());
                writeD(item.ObjID);
                writeD(item.Template.ItemID);
                writeD(item.Count);

                writeH(item.Template.Type2());
                writeH(item.CustomType1);

                writeD(item.Template.BodyPartId());
                writeH(item.Enchant);
                writeH(item.CustomType2);

                writeH(0x00);
            }
        }
    }
}