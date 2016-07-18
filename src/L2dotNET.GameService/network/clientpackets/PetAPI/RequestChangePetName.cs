using L2dotNET.GameService.Config;
using L2dotNET.GameService.Model.Playable;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets.PetAPI
{
    class RequestChangePetName : PacketBase
    {
        private string _name;
        private readonly GameClient _client;

        public RequestChangePetName(Packet packet, GameClient client)
        {
            _client = client;
            _name = packet.ReadString();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

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