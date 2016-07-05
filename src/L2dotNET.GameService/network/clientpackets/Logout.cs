using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.LoginAuth;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class Logout : GameServerNetworkRequest
    {
        public Logout(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        public override void Read()
        {
            // nothing
        }

        public override void Run()
        {
            AuthThread.Instance.SetInGameAccount(Client.AccountName);

            L2Player player = Client.CurrentPlayer;

            if (player == null) //re-login на выборе чаров
            {
                return;
            }

            if (player.PBlockAct == 1)
            {
                player.SendActionFailed();
                return;
            }

            if (player.isInCombat())
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.CantLogoutWhileFighting);
                player.SendActionFailed();
                return;
            }

            player.Termination();
            player.SendPacket(new LeaveWorld());
        }
    }
}