using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestAddTradeItem : GameServerNetworkRequest
    {
        private int sID;
        private long num;
        private int unk1;

        public RequestAddTradeItem(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            unk1 = readD(); // постоянно 1. в клиенте нет инфы что это
            sID = readD();
            num = readD();
        }

        public override void run()
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

            if (player.requester == null)
            {
                player.SendMessage("Your trade requestor has logged off.");
                player.SendActionFailed();
                player.TradeState = 0;
                return;
            }

            if ((player.TradeState == 4) || (player.requester.TradeState == 4)) // подтвердил уже
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.CANNOT_ADJUST_ITEMS_AFTER_TRADE_CONFIRMED);
                player.SendActionFailed();
                return;
            }

            L2Item item = player.Inventory.GetItemByObjectId(sID);

            if (item == null)
            {
                player.SendActionFailed();
                return;
            }

            if (num < 0)
                num = 1;

            if (num > item.Count)
                num = item.Count;

            if (!item.Template.isStackable() && (num > 1))
                num = 1;

            long numInList = player.AddItemToTrade(item.ObjId, num);
            long numCurrent = item.Count - numInList;
            player.SendPacket(new TradeOwnAdd(item, numInList));
            player.requester.SendPacket(new TradeOtherAdd(item, numInList));

            byte action = 2; //mod, 2-del
            if (item.Template.isStackable())
            {
                action = 3;
                if (numCurrent < 1)
                    action = 2;
            }

            player.SendPacket(new TradeUpdate(item, numCurrent, action));
        }
    }
}