using System.Collections.Generic;
using log4net;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Npcs;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestWarehouseDeposit : GameServerNetworkRequest
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RequestBypassToServer));

        public RequestWarehouseDeposit(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        private int _count;
        private long[] _items;

        public override void Read()
        {
            _count = ReadD();

            if ((_count < 0) || (_count > 255))
            {
                _count = 0;
            }

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

            L2Npc npc = player.FolkNpc;

            if (npc == null)
            {
                player.SendActionFailed();
                return;
            }

            int fee = _count * 30;
            int slots = 0;
            long adenatransfer = 0;
            for (int i = 0; i < _count; i++)
            {
                int objectId = (int)_items[i * 2];
                long count = _items[(i * 2) + 1];

                L2Item item = player.GetItemByObjId(objectId);

                if (item == null)
                {
                    Log.Info($"cant find item {objectId} in inventory {player.Name}");
                    player.SendActionFailed();
                    return;
                }

                if (item.Template.Stackable)
                {
                    slots += 1;
                }
                else
                {
                    slots += (int)count;
                }

                if (item.Template.ItemId == 57)
                {
                    adenatransfer += count;
                }
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

            List<long[]> transfer = new List<long[]>();
            for (int i = 0; i < _count; i++)
            {
                int objectId = (int)_items[i * 2];
                long count = _items[(i * 2) + 1];

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