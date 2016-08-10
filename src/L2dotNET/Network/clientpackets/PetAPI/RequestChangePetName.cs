using L2dotNET.model.playable;
using L2dotNET.model.player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets.PetAPI
{
    class RequestChangePetName : PacketBase
    {
        private readonly GameClient _client;
        private readonly string _name;

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