using L2dotNET.model.player;
using L2dotNET.Network.loginauth;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets
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

            if (player.Online == 1)
            {
                player.Online = 0;
                player.DeleteMe();
            }
            player.SendPacket(new LeaveWorld());
        }
    }
}