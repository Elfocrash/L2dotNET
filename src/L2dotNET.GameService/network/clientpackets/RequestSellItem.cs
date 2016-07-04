using System.Collections.Generic;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Npcs;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestSellItem : GameServerNetworkRequest
    {
        public RequestSellItem(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        private int _listId;
        private int _count;
        private long[] _items;

        public override void Read()
        {
            _listId = ReadD();
            _count = ReadD();
            _items = new long[_count * 3];

            for (int i = 0; i < _count; i++)
            {
                _items[i * 3 + 0] = ReadD();
                _items[i * 3 + 1] = ReadD();
                _items[i * 3 + 2] = ReadQ();
            }
        }

        public override void Run()
        {
            L2Player player = GetClient().CurrentPlayer;
            L2Npc npc = player.FolkNpc;

            if (npc == null)
            {
                player.SendActionFailed();
                return;
            }

            long totalCost = 0;
            int weight = 0;
            for (int i = 0; i < _count; i++)
            {
                int objectId = (int)_items[i * 3 + 0];
                long count = _items[i * 3 + 2];

                if ((count < 0) || (count > int.MaxValue))
                {
                    player.SendSystemMessage(SystemMessage.SystemMessageId.SellAttemptFailed);
                    player.SendActionFailed();
                    return;
                }

                L2Item item = player.Inventory.Items[objectId];

                if (item.Template.IsStackable())
                    totalCost += (int)(item.Count * (item.Template.Price * .5));
                else
                    totalCost += (int)(item.Template.Price * .5);

                weight += item.Template.Weight;
            }

            //if (totalCost > long.MaxValue)
            //{
            //    player.sendSystemMessage(SystemMessage.SystemMessageId.SELL_ATTEMPT_FAILED);
            //    player.sendActionFailed();
            //    return;
            //}

            int added,
                 currentAdena = player.GetAdena();
            if (currentAdena + totalCost >= int.MaxValue)
                added = int.MaxValue - currentAdena;
            else
                added = (int)totalCost;

            List<long[]> transfer = new List<long[]>();
            //InventoryUpdate iu = new InventoryUpdate();
            for (int i = 0; i < _count; i++)
            {
                int objectId = (int)_items[i * 3 + 0];
                long count = _items[i * 3 + 2];

                transfer.Add(new[] { objectId, count });
            }

            //player.Refund.transferHere(player, transfer, false);
            //player.Refund.validate();

            player.AddAdena(added,true);
            player.SendItemList(true);
            player.SendPacket(new ExBuySellListClose());

            if (weight != 0)
                player.UpdateWeight();

            //if (npc.Template.fnSell != null)
            //{
            //    player.sendPacket(new NpcHtmlMessage(player, npc.Template.fnSell, npc.ObjID, 0));
            //}
        }
    }
}