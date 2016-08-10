using System.Collections.Generic;
using L2dotNET.model.items;
using L2dotNET.model.npcs;
using L2dotNET.model.player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets
{
    class RequestSellItem : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _listId;
        private readonly int _count;
        private readonly int[] _items;

        public RequestSellItem(Packet packet, GameClient client)
        {
            _client = client;
            _listId = packet.ReadInt();
            _count = packet.ReadInt();
            _items = new int[_count * 3];

            for (int i = 0; i < _count; i++)
            {
                _items[(i * 3) + 0] = packet.ReadInt();
                _items[(i * 3) + 1] = packet.ReadInt();
                _items[(i * 3) + 2] = packet.ReadInt();
            }
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;
            L2Npc npc = player.FolkNpc;

            if (npc == null)
            {
                player.SendActionFailed();
                return;
            }

            int totalCost = 0;
            int weight = 0;
            for (int i = 0; i < _count; i++)
            {
                int objectId = _items[(i * 3) + 0];
                int count = _items[(i * 3) + 2];

                //if ((count < 0) || (count > int.MaxValue))
                if (count < 0)
                {
                    player.SendSystemMessage(SystemMessage.SystemMessageId.SellAttemptFailed);
                    player.SendActionFailed();
                    return;
                }

                L2Item item = player.Inventory.Items[objectId];

                if (item.Template.Stackable)
                    totalCost += (int)(item.Count * (item.Template.ReferencePrice * .5));
                else
                    totalCost += (int)(item.Template.ReferencePrice * .5);

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
            if ((currentAdena + totalCost) >= int.MaxValue)
                added = int.MaxValue - currentAdena;
            else
                added = totalCost;

            List<int[]> transfer = new List<int[]>();
            //InventoryUpdate iu = new InventoryUpdate();
            for (int i = 0; i < _count; i++)
            {
                int objectId = _items[(i * 3) + 0];
                int count = _items[(i * 3) + 2];

                transfer.Add(new[] { objectId, count });
            }

            //player.Refund.transferHere(player, transfer, false);
            //player.Refund.validate();

            player.AddAdena(added, true);
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