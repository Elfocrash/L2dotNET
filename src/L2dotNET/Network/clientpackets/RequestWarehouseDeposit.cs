﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Models.Items;
using L2dotNET.Models.Npcs;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;
using NLog;

namespace L2dotNET.Network.clientpackets
{
    class RequestWarehouseDeposit : PacketBase
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly GameClient _client;
        private readonly int _count;
        private readonly int[] _items;

        public RequestWarehouseDeposit(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
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

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                L2Player player = _client.CurrentPlayer;

                L2Npc npc = player.FolkNpc;

                if (npc == null)
                {
                    player.SendActionFailedAsync();
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
                        player.SendActionFailedAsync();
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
                    player.SendSystemMessage(SystemMessageId.YouNotEnoughAdenaPayFee);
                    player.SendActionFailedAsync();
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
                //    player.sendSystemMessage(SystemMessageId.WAREHOUSE_FULL);
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
            });
        }
    }
}