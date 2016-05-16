using L2dotNET.GameService.model.playable;
using L2dotNET.GameService.network.l2send;

namespace L2dotNET.GameService.network.l2recv
{
    class RequestChangePetName : GameServerNetworkRequest
    {
        private string name;
        public RequestChangePetName(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        public override void read()
        {
            name = readS();
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            if (player.Summon == null)
            {
                player.sendActionFailed();
                return;
            }

            if (!(player.Summon is L2Pet))
            {
                player.sendActionFailed();
                return;
            }

            if (name.Length > 8)
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.NAMING_PETNAME_UP_TO_8CHARS);
                player.sendActionFailed();
                return;
            }

            player.Summon.Name = name;
            ((L2Pet)player.Summon).sql_update();
            player.Summon.broadcastUserInfo();
        }
    }
}
