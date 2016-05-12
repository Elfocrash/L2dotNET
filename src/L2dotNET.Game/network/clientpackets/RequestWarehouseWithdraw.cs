using L2dotNET.GameService.model.inventory;
using L2dotNET.GameService.model.items;
using L2dotNET.GameService.model.npcs;
using log4net;
using System;
using System.Collections.Generic;

namespace L2dotNET.GameService.network.l2recv
{
    class RequestWarehouseWithdraw : GameServerNetworkRequest
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RequestBypassToServer));

        public RequestWarehouseWithdraw(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        private int _count;
        private int[] _items;
        public override void read()
        {
            _count = readD();

            if (_count < 0 || _count > 255)
            {
                _count = 0;
            }

            _items = new int[_count * 2];
            for (int i = 0; i < _count; i++)
            {
                _items[i * 2] = readD();
                _items[i * 2 + 1] = readD();
            }
        }

        public override void run()
        {
            L2Player player = getClient().CurrentPlayer;
            L2Citizen npc = player.FolkNpc;

            if (npc == null)
            {
                player.sendActionFailed();
                return;
            }

            int slots = 0;

            for (int i = 0; i < _count; i++)
            {
                int objectId = _items[i * 2];
                int count = _items[i * 2 + 1];

                L2Item item = (L2Item)player._warehouse.Items[objectId];

                if (item == null)
                {
                    log.Info($"cant find item { objectId } in warehouse { player.Name }");
                    player.sendActionFailed();
                    return;
                }

                if (item.Template.isStackable())
                    slots += 1;
                else
                    slots += count;
            }

            InvPrivateWarehouse pw = player._warehouse;
            int itsize = 0;
            if(pw == null)
                pw = new InvPrivateWarehouse(player);
            else
                itsize = pw.Items.Count;

            if (player.ItemLimit_Inventory < (player.getAllItems().Length + slots))
            {
                player.sendSystemMessage(129);//Your inventory is full.
                player.sendActionFailed();
                return;
            }

            List<int[]> transfer = new List<int[]>();
            for (int i = 0; i < _count; i++)
            {
                int objectId = _items[i * 2];
                int count = _items[i * 2 + 1];

                transfer.Add(new int[] { objectId, count });
            }

            pw.transferFrom(player, transfer, false);

            //if(npc.Template.fnBye != null)
            //{
            //    player.sendPacket(new NpcHtmlMessage(player, npc.Template.fnBye, npc.ObjID, 0));
            //}

            player.sendItemList(true);
        }
    }
}
