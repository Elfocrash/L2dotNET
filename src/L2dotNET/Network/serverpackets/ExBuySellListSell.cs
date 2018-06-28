using System.Collections.Generic;
using System.Linq;
using L2dotNET.Models.Items;
using L2dotNET.Models.Player;

namespace L2dotNET.Network.serverpackets
{
    class ExBuySellListSell : GameserverPacket
    {
        private readonly List<L2Item> _sells = new List<L2Item>();
        private readonly List<L2Item> _refund;

        public ExBuySellListSell(L2Player player)
        {
            foreach (L2Item item in player.GetAllItems().Where(item => !item.NotForTrade()))
                _sells.Add(item);

            // _refund = player.Refund._items;
        }

        public override void Write()
        {
            WriteByte(0xFE);
            WriteShort(0xB7);
            WriteInt(1);
            WriteShort(_sells.Count);

            foreach (L2Item item in _sells)
            {
                WriteInt(item.ObjId);
                WriteInt(item.Template.ItemId);
                WriteInt(0);
                WriteLong(item.Count);
                WriteShort(item.Template.Type2);
                WriteShort(item.CustomType1);
                WriteShort(0);
                WriteInt((int) item.Template.BodyPart);
                WriteShort(item.Enchant);
                WriteShort(item.CustomType2);
                WriteInt(item.AugmentationId);
                WriteInt(item.Durability);
                WriteInt(item.LifeTimeEnd());

                WriteShort(item.AttrAttackType);
                WriteShort(item.AttrAttackValue);

                WriteLong(item.Template.ReferencePrice / 2);
            }

            WriteShort(_refund.Count);

            int idx = 0;
            foreach (L2Item item in _refund)
            {
                WriteInt(item.ObjId);
                WriteInt(item.Template.ItemId);
                WriteInt(0);
                WriteLong(item.Count);
                WriteShort(item.Template.Type2);
                WriteShort(item.CustomType1);
                WriteShort(0);
                WriteInt((int) item.Template.BodyPart);
                WriteShort(item.Enchant);
                WriteShort(item.CustomType2);
                WriteInt(item.AugmentationId);
                WriteInt(item.Durability);
                WriteInt(item.LifeTimeEnd());

                WriteShort(item.AttrAttackType);
                WriteShort(item.AttrAttackValue);

                WriteInt(idx++);
                WriteLong((item.Template.ReferencePrice / 2) * item.Count);
            }

            WriteByte(0);
        }
    }
}