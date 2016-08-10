namespace L2dotNET.Network.clientpackets
{
    class RequestSendMsnChatLog : PacketBase
    {
        private readonly GameClient _client;
        private readonly string _text;
        private readonly string _email;
        private readonly int _type;

        public RequestSendMsnChatLog(Packet packet, GameClient client)
        {
            _client = client;
            _text = packet.ReadString();
            _email = packet.ReadString();
            _type = packet.ReadInt();
        }

        public override void RunImpl()
        {
            //L2Player player = getClient()._player;

            //todo log
        }
    }
}