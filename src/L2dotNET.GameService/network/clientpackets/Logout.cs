using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.LoginAuth;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class Logout : GameServerNetworkRequest
    {
        public Logout(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            // nothing
        }

        public override void run()
        {
            AuthThread.Instance.setInGameAccount(Client.AccountName);

            L2Player player = Client.CurrentPlayer;

            if (player == null) //re-login на выборе чаров
                return;

            if (player.PBlockAct == 1)
            {
                player.SendActionFailed();
                return;
            }

            if (player.isInCombat())
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.CANT_LOGOUT_WHILE_FIGHTING);
                player.SendActionFailed();
                return;
            }

            player.Termination();
            player.SendPacket(new LeaveWorld());
        }
    }
}