namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestSendMsnChatLog : GameServerNetworkRequest
    {
        public RequestSendMsnChatLog(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        private string _text,
                       _email;
        private int _type;

        public override void Read()
        {
            _text = ReadS();
            _email = ReadS();
            _type = ReadD();
        }

        public override void Run()
        {
            //            L2Player player = getClient()._player;

            //todo log
        }
    }
}