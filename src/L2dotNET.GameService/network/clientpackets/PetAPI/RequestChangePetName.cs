using L2dotNET.GameService.Model.Playable;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets.PetAPI
{
    class RequestChangePetName : GameServerNetworkRequest
    {
        private string name;

        public RequestChangePetName(GameClient client, byte[] data)
        {
            makeme(client, data);
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
                player.SendActionFailed();
                return;
            }

            if (!(player.Summon is L2Pet))
            {
                player.SendActionFailed();
                return;
            }

            if (name.Length > 8)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.NAMING_PETNAME_UP_TO_8CHARS);
                player.SendActionFailed();
                return;
            }

            player.Summon.Name = name;
            ((L2Pet)player.Summon).sql_update();
            player.Summon.BroadcastUserInfo();
        }
    }
}