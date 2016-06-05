using L2dotNET.GameService.model.items;
using L2dotNET.GameService.network.l2send;

namespace L2dotNET.GameService.network.l2recv
{
    class RequestAddTradeItem : GameServerNetworkRequest
    {
        private int sID;
        private long num;
        private int unk1;

        public RequestAddTradeItem(GameClient client, byte[] data)
        {
            base.makeme(client, data);
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
                player.sendActionFailed();
                return;
            }

            if (player.EnchantState != 0)
            {
                player.sendActionFailed();
                return;
            }

            if (player.requester == null)
            {
                player.sendMessage("Your trade requestor has logged off.");
                player.sendActionFailed();
                player.TradeState = 0;
                return;
            }

            if (player.TradeState == 4 || player.requester.TradeState == 4) // подтвердил уже
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.CANNOT_ADJUST_ITEMS_AFTER_TRADE_CONFIRMED);
                player.sendActionFailed();
                return;
            }

            L2Item item = player.Inventory.getByObject(sID);

            if (item == null)
            {
                player.sendActionFailed();
                return;
            }

            if (num < 0)
                num = 1;

            if (num > item.Count)
                num = item.Count;

            if (!item.Template.isStackable() && num > 1)
                num = 1;

            long numInList = player.AddItemToTrade(item.ObjID, num);
            long numCurrent = item.Count - numInList;
            player.sendPacket(new TradeOwnAdd(item, numInList));
            player.requester.sendPacket(new TradeOtherAdd(item, numInList));

            byte action = 2; //mod, 2-del
            if (item.Template.isStackable())
            {
                action = 3;
                if (numCurrent < 1)
                    action = 2;
            }

            player.sendPacket(new TradeUpdate(item, numCurrent, action));
        }
    }
}