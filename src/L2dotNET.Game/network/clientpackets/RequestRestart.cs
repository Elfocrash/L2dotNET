using L2dotNET.Game.network.l2send;
using L2dotNET.Game.world;
using L2dotNET.Game.tables;

namespace L2dotNET.Game.network.l2recv
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
                player.sendSystemMessage(102);//You cannot restart while in combat.
                player.sendActionFailed();
                return;
            }

            player.Termination();
            player.sendPacket(new RestartResponse());

            CharacterSelectionInfo csl = new CharacterSelectionInfo(Client.AccountName, Client._accountChars, Client.SessionId);
            csl.charId = player.ObjID;
            player.sendPacket(csl);
        }
    }
}
