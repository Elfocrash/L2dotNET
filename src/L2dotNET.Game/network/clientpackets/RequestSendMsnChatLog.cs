namespace L2dotNET.GameService.network.l2recv
{
    class RequestSendMsnChatLog : GameServerNetworkRequest
    {
        public RequestSendMsnChatLog(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        private string _text, _email;
        private int _type;

        public override void read()
        {
            _text = readS();
            _email = readS();
            _type = readD();
        }

        public override void run()
        {
            //            L2Player player = getClient()._player;

            //todo log              
        }
    }
}