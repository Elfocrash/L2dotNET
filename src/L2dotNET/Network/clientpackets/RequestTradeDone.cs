using L2dotNET.managers;
using L2dotNET.Models.player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets
{
    class RequestTradeDone : PacketBase
    {
        private readonly GameClient _client;
        private readonly bool _bDone;

        public RequestTradeDone(Packet packet, GameClient client)
        {
            _client = client;
            _bDone = packet.ReadInt() == 1;
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            if (player.TradeState < 3) // умник
            {
                player.SendActionFailed();
                return;
            }

            if (player.Requester == null)
            {
                player.SendMessage("Your trade requestor has logged off.");
                player.SendActionFailed();
                player.TradeState = 0;
                player.CurrentTrade?.Clear();

                return;
            }

            if (_bDone)
            {
                player.TradeState = 4;
                player.Requester.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.S1ConfirmedTrade).AddPlayerName(player.Name));

                if (player.Requester.TradeState == 4)
                    TradeManager.GetInstance().PersonalTrade(player, player.Requester);
            }
            else
            {
                TradeDone end = new TradeDone(false);
                player.TradeState = 0;
                player.CurrentTrade.Clear();
                player.SendPacket(end);
                player.Requester.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.S1CanceledTrade).AddPlayerName(player.Name));

                player.Requester.TradeState = 0;
                player.Requester.CurrentTrade.Clear();
                player.Requester.SendPacket(end);
                player.Requester = null;
            }
        }
    }
}