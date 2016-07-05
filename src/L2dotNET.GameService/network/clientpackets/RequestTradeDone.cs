using L2dotNET.GameService.Managers;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestTradeDone : GameServerNetworkRequest
    {
        private bool _bDone;

        public RequestTradeDone(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        public override void Read()
        {
            _bDone = ReadD() == 1;
        }

        public override void Run()
        {
            L2Player player = Client.CurrentPlayer;

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

                if (player.CurrentTrade != null)
                {
                    player.CurrentTrade.Clear();
                }

                return;
            }

            if (_bDone)
            {
                player.TradeState = 4;
                player.Requester.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.S1ConfirmedTrade).AddPlayerName(player.Name));

                if (player.Requester.TradeState == 4)
                {
                    TradeManager.GetInstance().PersonalTrade(player, player.Requester);
                }
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