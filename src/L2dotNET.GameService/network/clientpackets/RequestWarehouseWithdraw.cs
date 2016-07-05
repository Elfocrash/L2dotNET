using System.Collections.Generic;
using log4net;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Npcs;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestWarehouseWithdraw : GameServerNetworkRequest
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RequestBypassToServer));

        public RequestWarehouseWithdraw(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        private int _count;
        private int[] _items;

        public override void Read()
        {
            _count = ReadD();

            if ((_count < 0) || (_count > 255))
            {
                _count = 0;
            }

            _items = new int[_count * 2];
            for (int i = 0; i < _count; i++)
            {
                _items[i * 2] = ReadD();
                _items[(i * 2) + 1] = ReadD();
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

            int slots = 0;

            for (int i = 0; i < _count; i++)
            {
                int objectId = _items[i * 2];
                int count = _items[(i * 2) + 1];

                L2Item item = null; //player._warehouse.Items[objectId];

                if (item == null)
                {
                    Log.Info($"cant find item {objectId} in warehouse {player.Name}");
                    player.SendActionFailed();
                    return;
                }

                if (item.Template.IsStackable())
                {
                    slots += 1;
                }
                else
                {
                    slots += count;
                }
            }

            //InvPrivateWarehouse pw = player._warehouse ?? new InvPrivateWarehouse(player);
            //int itsize = 0;
            //else
            //    itsize = pw.Items.Count;

            if (player.ItemLimitInventory < (player.GetAllItems().Count + slots))
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.SlotsFull);
                player.SendActionFailed();
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
        }
    }
}