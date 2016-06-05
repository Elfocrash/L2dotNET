using L2dotNET.GameService.network.l2send;
using L2dotNET.GameService.model.items;
using L2dotNET.GameService.world;
using L2dotNET.GameService.model.npcs;
using System.Collections.Generic;

namespace L2dotNET.GameService.network.l2recv
{
    class RequestSellItem : GameServerNetworkRequest
    {
        public RequestSellItem(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        private int _listId;
        private int _count;
        private long[] _items;

        public override void read()
        {
            _listId = readD();
            _count = readD();
            _items = new long[_count * 3];

            for (int i = 0; i < _count; i++)
            {
                _items[i * 3 + 0] = readD();
                _items[i * 3 + 1] = readD();
                _items[i * 3 + 2] = readQ();
            }
        }

        public override void run()
        {
            L2Player player = getClient().CurrentPlayer;
            L2Npc npc = player.FolkNpc;

            if (npc == null)
            {
                player.sendActionFailed();
                return;
            }

            long totalCost = 0;
            int weight = 0;
            for (int i = 0; i < _count; i++)
            {
                int objectId = (int)_items[i * 3 + 0];
                long count = _items[i * 3 + 2];

                if (count < 0 || count > int.MaxValue)
                {
                    player.sendSystemMessage(SystemMessage.SystemMessageId.SELL_ATTEMPT_FAILED);
                    player.sendActionFailed();
                    return;
                }

                L2Item item = (L2Item)player.Inventory.Items[objectId];

                if (item.Template.isStackable())
                    totalCost += (int)(item.Count * (item.Template.Price * .5));
                else
                    totalCost += (int)(item.Template.Price * .5);

                weight += item.Template.Weight;
            }

            if (totalCost > long.MaxValue)
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.SELL_ATTEMPT_FAILED);
                player.sendActionFailed();
                return;
            }

            long added = 0, currentAdena = player.getAdena();
            if (currentAdena + totalCost >= int.MaxValue)
            {
                added = int.MaxValue - currentAdena;
            }
            else
                added = (int)totalCost;

            List<long[]> transfer = new List<long[]>();
            InventoryUpdate iu = new InventoryUpdate();
            for (int i = 0; i < _count; i++)
            {
                int objectId = (int)_items[i * 3 + 0];
                long count = _items[i * 3 + 2];

                transfer.Add(new long[] { objectId, count });
            }
            player.Refund.transferHere(player, transfer, false);
            player.Refund.validate();

            player.addAdena(added, false, false);
            player.sendItemList(true);
            player.sendPacket(new ExBuySellList_Close());

            if (weight != 0)
                player.updateWeight();

            //if (npc.Template.fnSell != null)
            //{
            //    player.sendPacket(new NpcHtmlMessage(player, npc.Template.fnSell, npc.ObjID, 0));
            //}
        }
    }
}
