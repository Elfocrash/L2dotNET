﻿using L2dotNET.Models.Items;

namespace L2dotNET.Network.serverpackets
{
    class TradeOtherAdd : GameserverPacket
    {
        private readonly L2Item _item;
        private int _num;

        public TradeOtherAdd(L2Item item, int num)
        {
            _item = item;
            _num = num;
        }

        public override void Write()
        {
            WriteByte(0x20);
            WriteShort(1);

            WriteShort(_item.Template.Type1);
            WriteInt(0); //item.ObjID
            WriteInt(_item.Template.ItemId);
            WriteInt(_item.Count);

            WriteShort(_item.Template.Type2);
            WriteShort(0); // ??

            WriteInt((int) _item.Template.BodyPart);
            WriteShort(_item.Enchant);
            WriteShort(0x00); // ?
            WriteShort(0x00);
        }
    }
}