using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestAddTradeItem : GameServerNetworkRequest
    {
        private int _sId;
        private long _num;
        private int _unk1;

        public RequestAddTradeItem(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        public override void Read()
        {
            _unk1 = ReadD(); // постоянно 1. в клиенте нет инфы что это
            _sId = ReadD();
            _num = ReadD();
        }

        public override void Run()
        {
            L2Player player = Client.CurrentPlayer;

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

            if (_num < 0)
            {
                _num = 1;
            }

            if (_num > item.Count)
            {
                _num = item.Count;
            }

            if (!item.Template.Stackable && (_num > 1))
            {
                _num = 1;
            }

            long numInList = player.AddItemToTrade(item.ObjId, _num);
            long numCurrent = item.Count - numInList;
            player.SendPacket(new TradeOwnAdd(item, numInList));
            player.Requester.SendPacket(new TradeOtherAdd(item, numInList));

            byte action = 2; //mod, 2-del
            if (item.Template.Stackable)
            {
                action = 3;
                if (numCurrent < 1)
                {
                    action = 2;
                }
            }

            player.SendPacket(new TradeUpdate(item, numCurrent, action));
        }
    }
}