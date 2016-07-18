using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets
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

            player.Termination();
            player.SendPacket(new RestartResponse());

            CharacterSelectionInfo csl = new CharacterSelectionInfo(_client.AccountName, _client.AccountChars, _client.SessionId)
                                         {
                                             CharId = player.ObjId
                                         };
            player.SendPacket(csl);
        }
    }
}