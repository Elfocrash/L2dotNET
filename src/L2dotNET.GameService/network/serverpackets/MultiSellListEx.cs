using L2dotNET.GameService.Tables.Multisell;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class MultiSellListEx : GameserverPacket
    {
        private readonly MultiSellList _list;

        public MultiSellListEx(MultiSellList list)
        {
            _list = list;
        }

        public override void Write()
        {
            WriteByte(0xd0);
            WriteInt(_list.Id);
            WriteInt(1); // page
            WriteInt(1); // finished
            WriteInt(_list.Container.Count); // size of pages 40
            WriteInt(_list.Container.Count);

            int inc = 0;
            foreach (MultiSellEntry entry in _list.Container)
            {
                WriteInt(inc);
                inc++;
                WriteByte(entry.Stackable);
                WriteShort(entry.Enchant);
                WriteInt(0x00); // C6
                WriteInt(0x00); // T1

                WriteShort(entry.AttrAttackType);
                WriteShort(entry.AttrAttackValue);
                WriteShort(entry.AttrDefenseValueFire);
                WriteShort(entry.AttrDefenseValueWater);
                WriteShort(entry.AttrDefenseValueWind);
                WriteShort(entry.AttrDefenseValueEarth);
                WriteShort(entry.AttrDefenseValueHoly);
                WriteShort(entry.AttrDefenseValueUnholy);

                WriteShort(entry.Give.Count);
                WriteShort(entry.Take.Count);

                foreach (MultiSellItem item in entry.Give)
                {
                    WriteInt(item.ItemId);
                    WriteInt(item.BodyPartId);
                    WriteShort(item.Type2);
                    WriteLong(item.Count);
                    WriteShort(item.Enchant);
                    WriteInt(item.Augment);
                    WriteInt(item.Durability);
                }

                foreach (MultiSellItem item in entry.Take)
                {
                    WriteInt(item.ItemId);
                    WriteShort(item.Type2);
                    WriteLong(item.Count);
                    WriteShort(item.Enchant);
                    WriteInt(item.Augment);
                    WriteInt(item.Durability);
                }
            }
        }
    }
}