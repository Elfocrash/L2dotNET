using System.Collections.Generic;
using System.Linq;
using L2dotNET.Models.Items;
using L2dotNET.Models.Player;

namespace L2dotNET.Network.serverpackets
{
    class ExQuestItemList : GameserverPacket
    {
        private readonly L2Item[] _items;
        private readonly List<int> _block = new List<int>();

        public ExQuestItemList(L2Player player)
        {
            _items = null; //player.getAllQuestItems();

            if (_items == null)
                return;

            foreach (L2Item item in _items.Where(item => item.Blocked))
                _block.Add(item.ObjId);
        }

        public override void Write()
        {
            //WriteByte(0xFE);
            //WriteShort(0xC5);
            //WriteShort(_items.Length);

            //foreach (L2Item item in _items)
            //{
            //    WriteInt(item.ObjId);
            //    WriteInt(item.Template.ItemId);
            //    WriteInt(0);
            //    WriteLong(item.Count);

            //    WriteShort(item.Template.Type2);
            //    WriteShort(0);
            //    WriteShort(item.IsEquipped);

            //    WriteInt(item.Template.BodyPart);
            //    WriteShort(item.Enchant);
            //    WriteShort(0);

            //    WriteInt(item.AugmentationId);
            //    WriteInt(item.Durability);
            //    WriteInt(item.LifeTimeEnd());

            //    WriteShort(item.AttrAttackType);
            //    WriteShort(item.AttrAttackValue);
            //    WriteShort(item.AttrDefenseValueFire);
            //    WriteShort(item.AttrDefenseValueWater);
            //    WriteShort(item.AttrDefenseValueWind);
            //    WriteShort(item.AttrDefenseValueEarth);
            //    WriteShort(item.AttrDefenseValueHoly);
            //    WriteShort(item.AttrDefenseValueUnholy);

            //    WriteShort(item.Enchant1);
            //    WriteShort(item.Enchant2);
            //    WriteShort(item.Enchant3);
            //}

            //WriteShort(_block.Count);
            //if (_block.Count <= 0)
            //    return;

            //WriteByte(1);
            //foreach (int id in _block)
            //    WriteInt(id);
        }
    }
}