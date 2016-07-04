using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.Tools;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class AnswerTradeRequest : GameServerNetworkRequest
    {
        private int response;

        public AnswerTradeRequest(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            response = readD();
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            if (player.TradeState != 2)
            {
                player.SendMessage("Stupid.");
                response = 0;
            }

            if (player.requester == null)
            {
                player.SendMessage("Your trade requestor has logged off.");
                player.SendActionFailed();
                player.TradeState = 0;
                return;
            }

            if ((response != 0) && (player.requester.TradeState != 1))
                response = 0;

            if ((response != 0) && (player.EnchantState != 0))
                response = 0;

            if ((response != 0) && !Calcs.checkIfInRange(150, player, player.requester, true))
                response = 0;

            switch (response)
            {
                case 0:
                    player.TradeState = 0;
                    player.requester.TradeState = 0;
                    player.requester.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.S1_DENIED_TRADE_REQUEST).AddPlayerName(player.Name));
                    player.requester.requester = null;
                    player.requester = null;
                    break;
                case 1:
                    player.requester.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.BEGIN_TRADE_WITH_S1).AddPlayerName(player.Name));
                    player.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.BEGIN_TRADE_WITH_S1).AddPlayerName(player.requester.Name));
                    player.TradeState = 3;
                    player.requester.TradeState = 3;
                    player.SendPacket(new TradeStart(player));
                    player.requester.SendPacket(new TradeStart(player.requester));
                    break;
            }
        }
    }
}