using System;
using System.Threading.Tasks;
using L2dotNET.Models.Items;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets
{
    class RequestAddTradeItem : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _sId;
        private int _num;
        private readonly int _unk1;

        public RequestAddTradeItem(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _unk1 = packet.ReadInt(); // постоянно 1. в клиенте нет инфы что это
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

                if (player.TradeState < 3) // умник
                {
                    player.SendActionFailedAsync();
                    return;
                }

                if (player.EnchantState != 0)
                {
                    player.SendActionFailedAsync();
                    return;
                }

                if (player.Requester == null)
                {
                    player.SendMessageAsync("Your trade requestor has logged off.");
                    player.SendActionFailedAsync();
                    player.TradeState = 0;
                    return;
                }

                if ((player.TradeState == 4) || (player.Requester.TradeState == 4)) // подтвердил уже
                {
                    player.SendSystemMessage(SystemMessage.SystemMessageId.CannotAdjustItemsAfterTradeConfirmed);
                    player.SendActionFailedAsync();
                    return;
                }

                L2Item item = player.Inventory.GetItemByObjectId(_sId);

                if (item == null)
                {
                    player.SendActionFailedAsync();
                    return;
                }

                if (_num > item.Count)
                    _num = item.Count;

                if (!item.Template.Stackable && (_num > 1))
                    _num = 1;

                int numInList = player.AddItemToTrade(item.CharacterId, _num);
                int numCurrent = item.Count - numInList;
                player.SendPacketAsync(new TradeOwnAdd(item, numInList));
                player.Requester.SendPacketAsync(new TradeOtherAdd(item, numInList));

                byte action = 2; //mod, 2-del
                if (item.Template.Stackable)
                {
                    action = (byte)(numCurrent < 1 ? 2 : 3);
                }

                player.SendPacketAsync(new TradeUpdate(item, numCurrent, 0));
            });
        }
    }
}