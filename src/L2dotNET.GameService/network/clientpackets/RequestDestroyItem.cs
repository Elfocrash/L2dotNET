using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestDestroyItem : GameServerNetworkRequest
    {
        public RequestDestroyItem(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        private int _sId;
        private int _num;

        public override void Read()
        {
            _sId = ReadD();
            _num = ReadD();
        }

        public override void Run()
        {
            L2Player player = GetClient().CurrentPlayer;

            if (player.PBlockAct == 1)
            {
                player.SendActionFailed();
                return;
            }

            if (player.TradeState != 0)
            {
                player.SendMessage("You cannot destroy items while trading.");
                player.SendActionFailed();
                return;
            }

            L2Item item = player.GetItemByObjId(_sId);

            if (item == null)
            {
                player.SendMessage("null item " + _sId);
                player.SendActionFailed();
                return;
            }

            if ((item.Template.CanEquipHero == 1) && (item.Template.Type == ItemTemplate.L2ItemType.Weapon))
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.HeroWeaponsCantDestroyed);
                player.SendActionFailed();
                return;
            }

            if (item.Template.IsDestruct == 0)
            {
                SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.S1S2);
                sm.AddItemName(item.Template.ItemId);
                sm.AddString("cannot be destroyed.");
                player.SendPacket(sm);
                player.SendActionFailed();
                return;
            }

            if (_num < 0)
            {
                _num = 1;
            }

            //if (item._isEquipped == 1)
            //{
            //    int pdollId = player.Inventory.getPaperdollId(item.Template);
            //    player.setPaperdoll(pdollId, null, true);
            //    player.broadcastUserInfo();
            //}

            player.DestroyItemById(item.Template.ItemId, _num);
        }
    }
}