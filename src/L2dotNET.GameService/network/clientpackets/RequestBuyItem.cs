using System.Linq;
using L2dotNET.GameService.Model.Npcs;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.Tables;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestBuyItem : GameServerNetworkRequest
    {
        private int _listId,
                    _count;
        private long[] _items;

        public RequestBuyItem(GameClient client, byte[] data)
        {
            makeme(client, data);
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
                player.SendActionFailed();
                return;
            }

            L2Npc trader = player.FolkNpc;

            if (trader == null)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.TRADE_ATTEMPT_FAILED);
                player.SendActionFailed();
                return;
            }

            ND_shop shop = NpcData.Instance._shops[trader.Template.NpcId];

            if (shop == null)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.TRADE_ATTEMPT_FAILED);
                player.SendActionFailed();
                return;
            }

            ND_shopList list = shop.lists[(short)_listId];

            if (list == null)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.TRADE_ATTEMPT_FAILED);
                player.SendActionFailed();
                return;
            }

            int adena = 0;
            int slots = 0,
                weight = 0;

            for (int i = 0; i < _count; i++)
            {
                int itemId = (int)_items[i * 2];

                bool notfound = true;
                foreach (ND_shopItem item in list.items.Where(item => item.item.ItemID == itemId))
                {
                    adena += item.item.Price * (int)_items[i * 2 + 1];

                    if (!item.item.isStackable())
                        slots++;
                    else
                    {
                        //if (!player.HasItem(item.item.ItemID))
                        //    slots++;
                    }

                    weight += (int)(item.item.Weight * _items[i * 2 + 1]);

                    notfound = false;
                    break;
                }

                if (notfound)
                {
                    player.SendSystemMessage(SystemMessage.SystemMessageId.TRADE_ATTEMPT_FAILED);
                    player.SendActionFailed();
                    return;
                }
            }

            if (adena > player.GetAdena())
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.YOU_NOT_ENOUGH_ADENA);
                return;
            }

            player.ReduceAdena(adena);

            for (int i = 0; i < _count; i++)
            {
                int itemId = (int)_items[i * 2];
                int count = (int)_items[i * 2 + 1];

                player.AddItem(itemId, count);
            }

            player.SendPacket(new ExBuySellList_Close());
        }
    }
}