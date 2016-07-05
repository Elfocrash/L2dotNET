using System.Collections.Generic;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class TradeStart : GameServerNetworkPacket
    {
        private L2Player _player;
        private readonly List<L2Item> _trade = new List<L2Item>();
        private readonly int _partnerId;

        public TradeStart(L2Player player)
        {
            this._player = player;
            _partnerId = player.Requester.ObjId;
            //foreach (L2Item item in player.getAllNonQuestItems().Where(item => (item.Template.is_trade != 0) && (item.AugmentationID <= 0) && (item._isEquipped != 1) && (item.Template.Type != ItemTemplate.L2ItemType.asset)))
            //    trade.Add(item);
        }

        protected internal override void Write()
        {
            WriteC(0x1E);
            WriteD(_partnerId);
            WriteH(_trade.Count);

            foreach (L2Item item in _trade)
            {
                WriteH(item.Template.Type1);
                WriteD(item.ObjId);
                WriteD(item.Template.ItemId);
                WriteD(item.Count);

                WriteH(item.Template.Type2);
                WriteH(item.CustomType1);

                WriteD(item.Template.BodyPart);
                WriteH(item.Enchant);
                WriteH(item.CustomType2);

                WriteH(0x00);
            }
        }
    }
}