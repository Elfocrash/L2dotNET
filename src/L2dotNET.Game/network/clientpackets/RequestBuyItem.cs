using L2dotNET.GameService.model.npcs;
using L2dotNET.GameService.tables;
using L2dotNET.GameService.network.l2send;

namespace L2dotNET.GameService.network.l2recv
{
    class RequestBuyItem : GameServerNetworkRequest
    {
        private int _listId, _count;
        private long[] _items;
        public RequestBuyItem(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        public override void read()
        {
            _listId = readD();
            _count = readD();


            _items = new long[_count * 2];

            for (int i = 0; i < _count; i++)
            {
                _items[i * 2] = readD();
                 _items[i * 2 + 1] = readQ();
            }
        }

        public override void run()
        {
            L2Player player = getClient().CurrentPlayer;

            if (_count <= 0)
            {
                player.sendActionFailed();
                return;
            }

            L2Citizen trader = player.FolkNpc;

            if (trader == null)
            {
                player.sendSystemMessage(1802); //The attempt to trade has failed.
                player.sendActionFailed();
                return;
            }

            ND_shop shop = NpcData.Instance._shops[trader.Template.NpcId];

            if (shop == null)
            {
                player.sendSystemMessage(1802); //The attempt to trade has failed.
                player.sendActionFailed();
                return;
            }

            ND_shopList list = shop.lists[(short)_listId];

            if (list == null)
            {
                player.sendSystemMessage(1802); //The attempt to trade has failed.
                player.sendActionFailed();
                return;
            }

            long adena = 0;
            int slots = 0, weight = 0;

            for (int i = 0; i < _count; i++)
            {
                int itemId = (int)_items[i * 2];

                bool notfound = true;
                foreach (ND_shopItem item in list.items)
                {
                    if (item.item.ItemID == itemId)
                    {
                        adena += item.item.Price * _items[i * 2 + 1];

                        if (!item.item.isStackable())
                            slots++;
                        else
                        {
                            if (!player.hasItem(item.item.ItemID))
                                slots++;
                        }

                        weight += (int)(item.item.Weight * _items[i * 2 + 1]);

                        notfound = false;
                        break;
                    }
                }

                if (notfound)
                {
                    player.sendSystemMessage(1802); //The attempt to trade has failed.
                    player.sendActionFailed();
                    return;
                }
            }

            if (adena > player.getAdena())
            {
                player.sendSystemMessage(279); //You do not have enough adena.
                return;
            }

            player.reduceAdena(adena, false, false);

            for (int i = 0; i < _count; i++)
            {
                int itemId = (int)_items[i * 2];
                long count = _items[i * 2 + 1];

                player.Inventory.addItem(itemId, count, false, false);
            }

            player.sendPacket(new ExBuySellList_Close());
        }
    }
}
