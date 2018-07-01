using System.Collections.Generic;
using System.Linq;
using L2dotNET.Models.Items;
using L2dotNET.Models.Player;

namespace L2dotNET.Network.serverpackets
{
    class SellList : GameserverPacket
    {
        private readonly List<L2Item> _sells = new List<L2Item>();
        private readonly int _adena;

        public SellList(L2Player player)
        {
            foreach (L2Item item in player.GetAllItems().Where(item => item.Template.Tradable && (item.AugmentationId <= 0) && (item.IsEquipped != 1)))
                _sells.Add(item);

            _adena = player.GetAdena();
        }

        public override void Write()
        {
            WriteByte(0x10);
            WriteInt(_adena);
            WriteInt(0);
            WriteShort(_sells.Count);

            foreach (L2Item item in _sells)
            {
                WriteInt(item.ObjId);
                WriteInt(item.Template.ItemId);
                WriteLong(item.Count);

                WriteShort(item.Template.Type2);
                WriteShort(item.Template.Type1);
                WriteInt((int) item.Template.BodyPart);

                WriteShort(item.Enchant);
                WriteShort(item.Template.Type2);
                WriteShort(0x00);
                WriteInt((int)(item.Template.ReferencePrice * 0.5));
            }
        }
    }
}