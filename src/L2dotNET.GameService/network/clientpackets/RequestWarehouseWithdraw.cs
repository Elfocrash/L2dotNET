using System.Collections.Generic;
using log4net;
using L2dotNET.GameService.Model.Inventory;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Npcs;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestWarehouseWithdraw : GameServerNetworkRequest
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RequestBypassToServer));

        public RequestWarehouseWithdraw(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        private int _count;
        private int[] _items;

        public override void read()
        {
            _count = readD();

            if ((_count < 0) || (_count > 255))
                _count = 0;

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
            L2Npc npc = player.FolkNpc;

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

                L2Item item = null;//player._warehouse.Items[objectId];

                if (item == null)
                {
                    log.Info($"cant find item {objectId} in warehouse {player.Name}");
                    player.sendActionFailed();
                    return;
                }

                if (item.Template.isStackable())
                    slots += 1;
                else
                    slots += count;
            }

            //InvPrivateWarehouse pw = player._warehouse ?? new InvPrivateWarehouse(player);
            //int itsize = 0;
            //else
            //    itsize = pw.Items.Count;

            if (player.ItemLimit_Inventory < (player.GetAllItems().Count + slots))
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.SLOTS_FULL);
                player.sendActionFailed();
                return;
            }

            List<int[]> transfer = new List<int[]>();
            for (int i = 0; i < _count; i++)
            {
                int objectId = _items[i * 2];
                int count = _items[i * 2 + 1];

                transfer.Add(new[] { objectId, count });
            }

            //pw.transferFrom(player, transfer, false);

            //if(npc.Template.fnBye != null)
            //{
            //    player.sendPacket(new NpcHtmlMessage(player, npc.Template.fnBye, npc.ObjID, 0));
            //}

            player.sendItemList(true);
        }
    }
}