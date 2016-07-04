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
            _items = null;//player.getAllQuestItems();

            foreach (L2Item item in _items.Where(item => item.Blocked))
                _block.Add(item.ObjId);
        }

        protected internal override void write()
        {
            writeC(0xFE);
            writeH(0xC5);
            writeH(_items.Length);

            foreach (L2Item item in _items)
            {
                writeD(item.ObjId);
                writeD(item.Template.ItemID);
                writeD(0);
                writeQ(item.Count);

                writeH(item.Template.Type2());
                writeH(0);
                writeH(item.IsEquipped);

                writeD(item.Template.BodyPartId());
                writeH(item.Enchant);
                writeH(0);

                writeD(item.AugmentationId);
                writeD(item.Durability);
                writeD(item.LifeTimeEnd());

                writeH(item.AttrAttackType);
                writeH(item.AttrAttackValue);
                writeH(item.AttrDefenseValueFire);
                writeH(item.AttrDefenseValueWater);
                writeH(item.AttrDefenseValueWind);
                writeH(item.AttrDefenseValueEarth);
                writeH(item.AttrDefenseValueHoly);
                writeH(item.AttrDefenseValueUnholy);

                writeH(item.Enchant1);
                writeH(item.Enchant2);
                writeH(item.Enchant3);
            }

            writeH(_block.Count);
            if (_block.Count > 0)
            {
                writeC(1);
                foreach (int id in _block)
                    writeD(id);
            }
        }
    }
}