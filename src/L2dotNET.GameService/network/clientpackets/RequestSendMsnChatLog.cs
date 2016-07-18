using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestSendMsnChatLog : PacketBase
    {
        private string _text,
                       _email;
        private int _type;
        private readonly GameClient _client;

        public RequestSendMsnChatLog(Packet packet, GameClient client)
        {
            _client = client;
            _text = packet.ReadString();
            _email = packet.ReadString();
            _type = packet.ReadInt();
        }

        public override void RunImpl()
        {
            //            L2Player player = getClient()._player;

            //todo log
        }
    }
}