using System;
using System.Threading.Tasks;
using L2dotNET.Models.Items;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets
{
    class RequestDestroyItem : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _sId;
        private readonly int _num;

        public RequestDestroyItem(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _sId = packet.ReadInt();
            _num = packet.ReadInt();
            if (_num < 0)
                _num = 1;
        }

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                L2Player player = _client.CurrentPlayer;

                if (player.PBlockAct == 1)
                {
                    player.SendActionFailedAsync();
                    return;
                }

                if (player.TradeState != 0)
                {
                    player.SendMessageAsync("You cannot destroy items while trading.");
                    player.SendActionFailedAsync();
                    return;
                }

                L2Item item = player.GetItemByObjId(_sId);

                if (item == null)
                {
                    player.SendMessageAsync($"null item {_sId}");
                    player.SendActionFailedAsync();
                    return;
                }

                if (!item.Template.Destroyable)
                {
                    SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.S1S2);
                    sm.AddItemName(item.Template.ItemId);
                    sm.AddString("cannot be destroyed.");
                    player.SendPacketAsync(sm);
                    player.SendActionFailedAsync();
                    return;
                }

                //if (item._isEquipped == 1)
                //{
                //    int pdollId = player.Inventory.getPaperdollId(item.Template);
                //    player.setPaperdoll(pdollId, null, true);
                //    player.broadcastUserInfo();
                //}

                player.DestroyItemById(item.Template.ItemId, _num);
            });
        }
    }
}