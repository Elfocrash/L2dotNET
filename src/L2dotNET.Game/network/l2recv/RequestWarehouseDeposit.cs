using L2dotNET.Game.network.l2send;
using L2dotNET.Game.model.items;
using L2dotNET.Game.model.inventory;
using System.Collections.Generic;
using L2dotNET.Game.model.npcs;
using L2dotNET.Game.world;
using System;

namespace L2dotNET.Game.network.l2recv
{
    class RequestWarehouseDeposit : GameServerNetworkRequest
    {
        public RequestWarehouseDeposit(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        private int _count;
        private long[] _items;
        public override void read()
        {
            _count = readD();

            if (_count < 0 || _count > 255)
            {
                _count = 0;
            }

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

            L2Citizen npc = player.FolkNpc;

            if (npc == null)
            {
                player.sendActionFailed();
                return;
            }

            long fee = _count * 30;
            int slots = 0;
            long adenatransfer = 0;
            for (int i = 0; i < _count; i++)
            {
                int objectId = (int)_items[i * 2];
                long count = _items[i * 2 + 1];

                L2Item item = player.getItemByObjId(objectId);

                if (item == null)
                {
                    Console.WriteLine("cant find item " + objectId + " in inventory " + player.Name);
                    player.sendActionFailed();
                    return;
                }

                if (item.Template.isStackable())
                    slots += 1;
                else
                    slots += (int)count;

                if (item.Template.ItemID == 57)
                    adenatransfer += count;
            }

            if ((player.getAdena() - adenatransfer) < fee)
            {
                player.sendSystemMessage(281);//You do not have enough adena to pay the fee.
                player.sendActionFailed();
                return;
            }

            InvPrivateWarehouse pw = player._warehouse;
            int itsize = 0;
            if(pw == null)
                pw = new InvPrivateWarehouse(player);
            else
                itsize = pw.Items.Count;

            if (player.ItemLimit_Warehouse < (itsize + slots))
            {
                player.sendSystemMessage(130);//Your warehouse is full.
                player.sendActionFailed();
                return;
            }

            player.reduceAdena(fee, true, false);

            List<long[]> transfer = new List<long[]>();
            for (int i = 0; i < _count; i++)
            {
                int objectId = (int)_items[i * 2];
                long count = _items[i * 2 + 1];

                transfer.Add(new long[] { objectId, count });
            }

            pw.transferHere(player, transfer, false);

            //if(npc.Template.fnBye != null)
            //{
            //    player.sendPacket(new NpcHtmlMessage(player, npc.Template.fnBye, npc.ObjID, 0));
            //}

            player.sendItemList(true);
        }
    }
}
