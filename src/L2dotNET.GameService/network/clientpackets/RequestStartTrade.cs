using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.Tools;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestStartTrade : GameServerNetworkRequest
    {
        private int _targetId;

        public RequestStartTrade(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        public override void Read()
        {
            _targetId = ReadD();
        }

        public override void Run()
        {
            L2Player player = Client.CurrentPlayer;

            if (player.TradeState != 0)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.AlreadyTrading);
                player.SendActionFailed();
                return;
            }

            if (player.ObjId == _targetId)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.CannotUseOnYourself);
                player.SendActionFailed();
                return;
            }

            if (player.EnchantState != 0)
            {
                player.SendActionFailed();
                return;
            }

            if ((player.CurrentTarget == null) || !(player.CurrentTarget is L2Player))
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.TargetIsIncorrect);
                player.SendActionFailed();
                return;
            }

            L2Player target = (L2Player)player.CurrentTarget;
            if (target.TradeState != 0)
            {
                player.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.S1AlreadyTrading).AddPlayerName(target.Name));
                player.SendActionFailed();
                return;
            }

            if (target.PartyState == 1)
            {
                player.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.S1IsBusyTryLater).AddPlayerName(target.Name));
                player.SendActionFailed();
                return;
            }

            if (!Calcs.CheckIfInRange(150, player, target, true))
            {
                player.SendActionFailed();
                return;
            }

            player.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.RequestS1ForTrade).AddPlayerName(target.Name));
            target.Requester = player;
            player.Requester = target;
            target.SendPacket(new SendTradeRequest(player.ObjId));
            target.TradeState = 2; // жмакает ответ
            player.TradeState = 1; // запросил
        }
    }
}