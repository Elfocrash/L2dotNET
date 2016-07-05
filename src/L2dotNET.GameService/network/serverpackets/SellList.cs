using System.Collections.Generic;
using System.Linq;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class SellList : GameServerNetworkPacket
    {
        private readonly List<L2Item> _sells = new List<L2Item>();
        private readonly int _adena;

        public SellList(L2Player player, int npcObj)
        {
            foreach (L2Item item in player.GetAllItems().Where(item => (item.Template.Tradable) && (item.AugmentationId <= 0) && (item.IsEquipped != 1)))
            {
                _sells.Add(item);
            }

            _adena = player.GetAdena();
        }

        protected internal override void Write()
        {
            WriteC(0x10);
            WriteD(_adena);
            WriteD(0);
            WriteH(_sells.Count);

            foreach (L2Item item in _sells)
            {
                WriteD(item.ObjId);
                WriteD(item.Template.ItemId);
                WriteQ(item.Count);

                WriteH(item.Template.Type2);
                WriteH(item.Template.Type1);
                WriteD(item.Template.BodyPart);

                WriteH(item.Enchant);
                WriteH(item.Template.Type2);
                WriteH(0x00);
                WriteD((int)(item.Template.ReferencePrice * 0.5));
            }
        }
    }
}