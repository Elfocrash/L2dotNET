﻿using System.Collections.Generic;
using L2dotNET.Models.Items;

namespace L2dotNET.Network.serverpackets
{
    class ExPostItemList : GameserverPacket
    {
        private readonly List<L2Item> _list;

        public ExPostItemList(List<L2Item> list)
        {
            _list = list;
        }

        public override void Write()
        {
            WriteByte(0xfe);
            WriteShort(0xb2);
            WriteInt(_list.Count);

            foreach (L2Item item in _list)
            {
                WriteInt(item.ObjectId);
                WriteInt(item.Template.ItemId);
                WriteInt(0);
                WriteLong(item.Count);

                WriteShort(item.Template.Type2);
                WriteShort(0);
                WriteShort(0);

                WriteInt((int) item.Template.BodyPart);
                WriteShort(item.Enchant);
                WriteShort(0);

                WriteInt(item.AugmentationId);
                WriteInt(item.Durability);
                WriteInt(item.LifeTimeEnd());

                WriteShort(item.AttrAttackType);
                WriteShort(item.AttrAttackValue);

            }
        }
    }
}