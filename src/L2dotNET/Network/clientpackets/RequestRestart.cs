using L2dotNET.Models.player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets
{
    class RequestRestart : PacketBase
    {
        private readonly GameClient _client;

        public RequestRestart(Packet packet, GameClient client)
        {
            _client = client;
        }

        public override void RunImpl()
        {
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
                player.SendSystemMessage(SystemMessage.SystemMessageId.CantRestartWhileFighting);
                player.SendActionFailed();
                return;
            }

            player.Online = 0;
            player.DeleteMe();
            player.SendPacket(new RestartResponse());

            CharacterSelectionInfo csl = new CharacterSelectionInfo(_client.AccountName, _client.AccountChars, _client.SessionKey.PlayOkId1)
            {
                CharId = player.ObjId
            };
            player.SendPacket(csl);
        }
    }
}