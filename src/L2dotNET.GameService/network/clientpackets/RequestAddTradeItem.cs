using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestAddTradeItem : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _sId;
        private int _num;
        private readonly int _unk1;

        public RequestAddTradeItem(Packet packet, GameClient client)
        {
            _client = client;
            _unk1 = packet.ReadInt(); // постоянно 1. в клиенте нет инфы что это
            _sId = packet.ReadInt();
            _num = packet.ReadInt();
            if (_num < 0)
                _num = 1;
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            if (player.TradeState < 3) // умник
            {
                player.SendActionFailed();
                return;
            }

            if (player.EnchantState != 0)
            {
                player.SendActionFailed();
                return;
            }

            if (player.Requester == null)
            {
                player.SendMessage("Your trade requestor has logged off.");
                player.SendActionFailed();
                player.TradeState = 0;
                return;
            }

            if ((player.TradeState == 4) || (player.Requester.TradeState == 4)) // подтвердил уже
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.CannotAdjustItemsAfterTradeConfirmed);
                player.SendActionFailed();
                return;
            }

            L2Item item = player.Inventory.GetItemByObjectId(_sId);

            if (item == null)
            {
                player.SendActionFailed();
                return;
            }

            if (_num > item.Count)
                _num = item.Count;

            if (!item.Template.Stackable && (_num > 1))
                _num = 1;

            int numInList = player.AddItemToTrade(item.ObjId, _num);
            int numCurrent = item.Count - numInList;
            player.SendPacket(new TradeOwnAdd(item, numInList));
            player.Requester.SendPacket(new TradeOtherAdd(item, numInList));

            byte action = 2; //mod, 2-del
            if (item.Template.Stackable)
            {
                action = 3;
                if (numCurrent < 1)
                    action = 2;
            }

            player.SendPacket(new TradeUpdate(item, numCurrent, 0));
        }
    }
}