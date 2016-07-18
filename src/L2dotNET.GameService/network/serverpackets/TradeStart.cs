using System.Collections.Generic;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class TradeStart : GameserverPacket
    {
        private L2Player _player;
        private readonly List<L2Item> _trade = new List<L2Item>();
        private readonly int _partnerId;

        public TradeStart(L2Player player)
        {
            _player = player;
            _partnerId = player.Requester.ObjId;
            //foreach (L2Item item in player.getAllNonQuestItems().Where(item => (item.Template.is_trade != 0) && (item.AugmentationID <= 0) && (item._isEquipped != 1) && (item.Template.Type != ItemTemplate.L2ItemType.asset)))
            //    trade.Add(item);
        }

        protected internal override void Write()
        {
            WriteByte(0x1E);
            WriteInt(_partnerId);
            WriteShort(_trade.Count);

            foreach (L2Item item in _trade)
            {
                WriteShort(item.Template.Type1);
                WriteInt(item.ObjId);
                WriteInt(item.Template.ItemId);
                WriteInt(item.Count);

                WriteShort(item.Template.Type2);
                WriteShort(item.CustomType1);

                WriteInt(item.Template.BodyPart);
                WriteShort(item.Enchant);
                WriteShort(item.CustomType2);

                WriteShort(0x00);
            }
        }
    }
}