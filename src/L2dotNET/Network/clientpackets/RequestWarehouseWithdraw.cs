using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using L2dotNET.Logging.Abstraction;
using L2dotNET.Models.Items;
using L2dotNET.Models.Npcs;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets
{
    class RequestWarehouseWithdraw : PacketBase
    {
        private static readonly ILog Log = LogProvider.GetCurrentClassLogger();

        private readonly int _count;
        private readonly int[] _items;
        private readonly GameClient _client;

        public RequestWarehouseWithdraw(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
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

                int slots = 0;

                for (int i = 0; i < _count; i++)
                {
                    int objectId = _items[i * 2];
                    int count = _items[(i * 2) + 1];

                    L2Item item = null; //player._warehouse.Items[objectId];

                    if (item == null)
                    {
                        Log.Info($"cant find item {objectId} in warehouse {player.Name}");
                        player.SendActionFailedAsync();
                        return;
                    }

                    if (item.Template.Stackable)
                        slots += 1;
                    else
                        slots += count;
                }

                //InvPrivateWarehouse pw = player._warehouse ?? new InvPrivateWarehouse(player);
                //int itsize = 0;
                //else
                //    itsize = pw.Items.Count;

                if (player.ItemLimitInventory < (player.GetAllItems().Count + slots))
                {
                    player.SendSystemMessage(SystemMessage.SystemMessageId.SlotsFull);
                    player.SendActionFailedAsync();
                    return;
                }

                List<int[]> transfer = new List<int[]>();
                for (int i = 0; i < _count; i++)
                {
                    int objectId = _items[i * 2];
                    int count = _items[(i * 2) + 1];

                    transfer.Add(new[] { objectId, count });
                }

                //pw.transferFrom(player, transfer, false);

                //if(npc.Template.fnBye != null)
                //{
                //    player.sendPacket(new NpcHtmlMessage(player, npc.Template.fnBye, npc.ObjID, 0));
                //}

                player.SendItemList(true);
            });
        }
    }
}