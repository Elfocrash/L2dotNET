using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestRestart : GameServerNetworkRequest
    {
        public RequestRestart(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            // do nothing
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            if (player.PBlockAct == 1)
            {
                player.SendActionFailed();
                return;
            }

            if (player.isInCombat())
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.CANT_RESTART_WHILE_FIGHTING);
                player.SendActionFailed();
                return;
            }

            player.Termination();
            player.SendPacket(new RestartResponse());

            CharacterSelectionInfo csl = new CharacterSelectionInfo(Client.AccountName, Client.AccountChars, Client.SessionId);
            csl.charId = player.ObjId;
            player.SendPacket(csl);
        }
    }
}