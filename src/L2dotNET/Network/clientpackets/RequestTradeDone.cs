using System;
using System.Threading.Tasks;
using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Managers;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets
{
    class RequestTradeDone : PacketBase
    {
        private readonly GameClient _client;
        private readonly bool _bDone;

        public RequestTradeDone(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _bDone = packet.ReadInt() == 1;
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

                if (player.Requester == null)
                {
                    player.SendMessageAsync("Your trade requestor has logged off.");
                    player.SendActionFailedAsync();
                    player.TradeState = 0;
                    player.CurrentTrade?.Clear();

                    return;
                }

                if (_bDone)
                {
                    player.TradeState = 4;
                    player.Requester.SendPacketAsync(new SystemMessage(SystemMessageId.S1ConfirmedTrade).AddPlayerName(player.Name));

                    if (player.Requester.TradeState == 4)
                        TradeManager.GetInstance().PersonalTrade(player, player.Requester);
                }
                else
                {
                    TradeDone end = new TradeDone(false);
                    player.TradeState = 0;
                    player.CurrentTrade.Clear();
                    player.SendPacketAsync(end);
                    player.Requester.SendPacketAsync(new SystemMessage(SystemMessageId.S1CanceledTrade).AddPlayerName(player.Name));

                    player.Requester.TradeState = 0;
                    player.Requester.CurrentTrade.Clear();
                    player.Requester.SendPacketAsync(end);
                    player.Requester = null;
                }
            });
        }
    }
}