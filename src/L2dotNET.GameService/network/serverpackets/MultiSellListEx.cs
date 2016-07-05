using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Tables.Multisell;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class MultiSellListEx : GameServerNetworkPacket
    {
        private readonly MultiSellList _list;

        public MultiSellListEx(L2Player player, MultiSellList list)
        {
            this._list = list;
        }

        protected internal override void Write()
        {
            WriteC(0xd0);
            WriteD(_list.Id);
            WriteD(1); // page
            WriteD(1); // finished
            WriteD(_list.Container.Count); // size of pages 40
            WriteD(_list.Container.Count);

            int inc = 0;
            foreach (MultiSellEntry entry in _list.Container)
            {
                WriteD(inc);
                inc++;
                WriteC(entry.Stackable);
                WriteH(entry.Enchant);
                WriteD(0x00); // C6
                WriteD(0x00); // T1

                WriteH(entry.AttrAttackType);
                WriteH(entry.AttrAttackValue);
                WriteH(entry.AttrDefenseValueFire);
                WriteH(entry.AttrDefenseValueWater);
                WriteH(entry.AttrDefenseValueWind);
                WriteH(entry.AttrDefenseValueEarth);
                WriteH(entry.AttrDefenseValueHoly);
                WriteH(entry.AttrDefenseValueUnholy);

                WriteH(entry.Give.Count);
                WriteH(entry.Take.Count);

                foreach (MultiSellItem item in entry.Give)
                {
                    WriteD(item.ItemId);
                    WriteD(item.BodyPartId);
                    WriteH(item.Type2);
                    WriteQ(item.Count);
                    WriteH(item.Enchant);
                    WriteD(item.Augment);
                    WriteD(item.Durability);
                }

                foreach (MultiSellItem item in entry.Take)
                {
                    WriteD(item.ItemId);
                    WriteH(item.Type2);
                    WriteQ(item.Count);
                    WriteH(item.Enchant);
                    WriteD(item.Augment);
                    WriteD(item.Durability);
                }
            }
        }
    }
}