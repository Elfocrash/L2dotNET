using L2dotNET.GameService.network.l2send;

namespace L2dotNET.GameService.network.l2recv
{
    class RequestRestart : GameServerNetworkRequest
    {
        public RequestRestart(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        public override void read()
        {
            // do nothing
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            if (player._p_block_act == 1)
            {
                player.sendActionFailed();
                return;
            }

            if (player.isInCombat())
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.CANT_RESTART_WHILE_FIGHTING);
                player.sendActionFailed();
                return;
            }

            player.Termination();
            player.sendPacket(new RestartResponse());

            CharacterSelectionInfo csl = new CharacterSelectionInfo(Client.AccountName, Client.AccountChars, Client.SessionId);
            csl.charId = player.ObjID;
            player.sendPacket(csl);
        }
    }
}
