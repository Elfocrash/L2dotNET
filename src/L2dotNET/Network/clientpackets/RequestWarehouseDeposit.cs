using System.Collections.Generic;
using log4net;
using L2dotNET.model.items;
using L2dotNET.model.npcs;
using L2dotNET.model.player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets
{
    class RequestWarehouseDeposit : PacketBase
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RequestBypassToServer));

        private readonly GameClient _client;
        private readonly int _count;
        private readonly int[] _items;

        public RequestWarehouseDeposit(Packet packet, GameClient client)
        {
            _client = client;
            _count = packet.ReadInt();

            if ((_count < 0) || (_count > 255))
                _count = 0;

            _items = new int[_count * 2];
            for (int i = 0; i < _count; i++)
            {
                _items[i * 2] = packet.ReadInt();
                _items[(i * 2) + 1] = packet.ReadInt();
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

            int fee = _count * 30;
            int slots = 0;
            int adenatransfer = 0;
            for (int i = 0; i < _count; i++)
            {
                int objectId = _items[i * 2];
                int count = _items[(i * 2) + 1];

                L2Item item = player.GetItemByObjId(objectId);

                if (item == null)
                {
                    Log.Info($"cant find item {objectId} in inventory {player.Name}");
                    player.SendActionFailed();
                    return;
                }

                if (item.Template.Stackable)
                    slots += 1;
                else
                    slots += count;

                if (item.Template.ItemId == 57)
                    adenatransfer += count;
            }

            if ((player.GetAdena() - adenatransfer) < fee)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.YouNotEnoughAdenaPayFee);
                player.SendActionFailed();
                return;
            }

            //InvPrivateWarehouse pw = player._warehouse;
            //int itsize = 0;
            //if (pw == null)
            //    pw = new InvPrivateWarehouse(player);
            //else
            //    itsize = pw.Items.Count;

            //if (player.ItemLimit_Warehouse < (itsize + slots))
            //{
            //    player.sendSystemMessage(SystemMessage.SystemMessageId.WAREHOUSE_FULL);
            //    player.sendActionFailed();
            //    return;
            //}

            player.ReduceAdena(fee);

            List<int[]> transfer = new List<int[]>();
            for (int i = 0; i < _count; i++)
            {
                int objectId = _items[i * 2];
                int count = _items[(i * 2) + 1];

                transfer.Add(new[] { objectId, count });
            }

            // pw.transferHere(player, transfer, false);

            //if(npc.Template.fnBye != null)
            //{
            //    player.sendPacket(new NpcHtmlMessage(player, npc.Template.fnBye, npc.ObjID, 0));
            //}

            player.SendItemList(true);
        }
    }
}