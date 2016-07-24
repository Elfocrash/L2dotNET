using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.LoginAuth;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class Logout : PacketBase
    {
        private readonly GameClient _client;

        public Logout(Packet packet, GameClient client)
        {
            _client = client;
        }

        public override void RunImpl()
        {
            AuthThread.Instance.SetInGameAccount(_client.AccountName);

            L2Player player = _client.CurrentPlayer;

            if (player == null)
                return;

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

            player.DeleteMe();
            player.SendPacket(new LeaveWorld());
        }
    }
}