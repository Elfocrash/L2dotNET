using System;
using L2dotNET.Models.Inventory;
using L2dotNET.Models.Items;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;
using L2dotNET.Plugins;

namespace L2dotNET.Network.clientpackets
{
    class RequestUseItem : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _sId;

        public RequestUseItem(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _sId = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            if (player.PBlockAct == 1)
            {
                player.SendActionFailed();
                return;
            }

            L2Item item = player.GetItemByObjId(_sId);

            if (item == null)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.IncorrectItem);
                return;
            }

            if (player.TradeState != 0)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.CannotPickupOrUseItemWhileTrading);
                player.SendActionFailed();
                return;
            }

            if (item.IsEquipable())
            {
                if (!item.Equipped)
                {
                    if (player.Inventory.Paperdoll[Inventory.GetPaperdollIndex(item.Template.BodyPart)] != null)
                    {
                        var equipped = player.Inventory.Paperdoll[Inventory.GetPaperdollIndex(item.Template.BodyPart)];
                        equipped.Unequip(player);
                    }
                    player.Inventory.Paperdoll[Inventory.GetPaperdollIndex(item.Template.BodyPart)] = item;
                    item.Equip(player);
                    player.BroadcastUserInfo();
                    player.SendPacket(new ItemList(player, true));
                    foreach (IPlugin plugin in PluginManager.Instance.Plugins)
                        plugin.OnItemEquip(player, item);
                }
                else
                {
                    player.Inventory.Paperdoll[Inventory.GetPaperdollIndex(item.Template.BodyPart)] = null;
                    item.Unequip(player);
                    player.BroadcastUserInfo();
                    player.SendPacket(new ItemList(player, true));
                }
            }
            else
            {
                ItemHandler.Instance.Process(player, item);
            }
        }

    }
}