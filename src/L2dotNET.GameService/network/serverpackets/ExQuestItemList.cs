using System.Collections.Generic;
using System.Linq;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExQuestItemList : GameServerNetworkPacket
    {
        private readonly L2Item[] _items;
        private readonly List<int> _block = new List<int>();

        public ExQuestItemList(L2Player player)
        {
            _items = null; //player.getAllQuestItems();

            foreach (L2Item item in _items.Where(item => item.Blocked))
            {
                _block.Add(item.ObjId);
            }
        }

        protected internal override void Write()
        {
            WriteC(0xFE);
            WriteH(0xC5);
            WriteH(_items.Length);

            foreach (L2Item item in _items)
            {
                WriteD(item.ObjId);
                WriteD(item.Template.ItemId);
                WriteD(0);
                WriteQ(item.Count);

                WriteH(item.Template.Type2());
                WriteH(0);
                WriteH(item.IsEquipped);

                WriteD(item.Template.BodyPartId());
                WriteH(item.Enchant);
                WriteH(0);

                WriteD(item.AugmentationId);
                WriteD(item.Durability);
                WriteD(item.LifeTimeEnd());

                WriteH(item.AttrAttackType);
                WriteH(item.AttrAttackValue);
                WriteH(item.AttrDefenseValueFire);
                WriteH(item.AttrDefenseValueWater);
                WriteH(item.AttrDefenseValueWind);
                WriteH(item.AttrDefenseValueEarth);
                WriteH(item.AttrDefenseValueHoly);
                WriteH(item.AttrDefenseValueUnholy);

                WriteH(item.Enchant1);
                WriteH(item.Enchant2);
                WriteH(item.Enchant3);
            }

            WriteH(_block.Count);
            if (_block.Count > 0)
            {
                WriteC(1);
                foreach (int id in _block)
                {
                    WriteD(id);
                }
            }
        }
    }
}