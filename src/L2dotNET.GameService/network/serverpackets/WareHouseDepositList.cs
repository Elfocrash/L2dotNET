using System.Collections.Generic;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class WareHouseDepositList : GameServerNetworkPacket
    {
        public static short WhPrivate = 1;
        public static short WhClan = 2;
        public static short WhCastle = 3;
        public static short WhFreight = 4;
        private readonly List<L2Item> _items;
        private readonly short _type;
        private readonly long _adena;

        public WareHouseDepositList(L2Player player, List<L2Item> items, short type)
        {
            _type = type;
            _adena = player.GetAdena();
            _items = items;
        }

        protected internal override void Write()
        {
            WriteC(0x41);
            WriteH(_type);
            WriteD(_adena);
            WriteH(_items.Count);

            foreach (L2Item item in _items)
            {
                WriteH(item.Template.Type1);
                WriteD(item.ObjId);
                WriteD(item.Template.ItemId);
                WriteD(item.Count);
                WriteH(item.Template.Type2);
                WriteH(0); //custom type 1
                WriteD(item.Template.BodyPart);
                WriteH(item.Enchant);
                WriteH(0); //custom type 2
                WriteH(0);
                //writeD(item.AugmentationID);
                WriteD(item.ObjId);
                WriteQ(0x00);
                _items.Clear();
            }
        }
    }
}