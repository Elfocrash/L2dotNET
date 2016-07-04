using L2dotNET.GameService.Model.Playable;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets.PetAPI
{
    class RequestChangePetName : GameServerNetworkRequest
    {
        private string _name;

        public RequestChangePetName(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        public override void Read()
        {
            _name = ReadS();
        }

        public override void Run()
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

            if (_name.Length > 8)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.NamingPetnameUpTo_8Chars);
                player.SendActionFailed();
                return;
            }

            player.Summon.Name = _name;
            ((L2Pet)player.Summon).sql_update();
            player.Summon.BroadcastUserInfo();
        }
    }
}