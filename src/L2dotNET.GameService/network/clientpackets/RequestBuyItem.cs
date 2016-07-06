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
            Makeme(client, data);
        }

        public override void Read()
        {
            _listId = ReadD();
            _count = ReadD();

            _items = new long[_count * 2];

            for (int i = 0; i < _count; i++)
            {
                _items[i * 2] = ReadD();
                _items[(i * 2) + 1] = ReadQ();
            }
        }

        public override void Run()
        {
            L2Player player = GetClient().CurrentPlayer;

            if (_count <= 0)
            {
                player.SendActionFailed();
                return;
            }

            L2Npc trader = player.FolkNpc;

            if (trader == null)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.TradeAttemptFailed);
                player.SendActionFailed();
                return;
            }

            NdShop shop = NpcData.Instance.Shops[trader.Template.NpcId];

            if (shop == null)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.TradeAttemptFailed);
                player.SendActionFailed();
                return;
            }

            NdShopList list = shop.Lists[(short)_listId];

            if (list == null)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.TradeAttemptFailed);
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
                foreach (NdShopItem item in list.Items.Where(item => item.Item.ItemId == itemId))
                {
                    adena += item.Item.ReferencePrice * (int)_items[(i * 2) + 1];

                    if (!item.Item.Stackable)
                    {
                        slots++;
                    }
                    //else
                    //{
                    //    if (!player.HasItem(item.item.ItemID))
                    //        slots++;
                    //}

                    weight += (int)(item.Item.Weight * _items[(i * 2) + 1]);

                    notfound = false;
                    break;
                }

                if (notfound)
                {
                    player.SendSystemMessage(SystemMessage.SystemMessageId.TradeAttemptFailed);
                    player.SendActionFailed();
                    return;
                }
            }

            if (adena > player.GetAdena())
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.YouNotEnoughAdena);
                return;
            }

            player.ReduceAdena(adena);

            for (int i = 0; i < _count; i++)
            {
                int itemId = (int)_items[i * 2];
                int count = (int)_items[(i * 2) + 1];

                player.AddItem(itemId, count);
            }

            player.SendPacket(new ExBuySellListClose());
        }
    }
}