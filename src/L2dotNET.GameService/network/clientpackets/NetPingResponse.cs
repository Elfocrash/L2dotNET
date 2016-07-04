namespace L2dotNET.GameService.Network.Clientpackets
{
    class NetPingResponse : GameServerNetworkRequest
    {
        private int _request;
        private int _msec;
        private int _unk2;

        public NetPingResponse(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        public override void Read()
        {
            _request = ReadD();
            _msec = ReadD();
            _unk2 = ReadD();
        }

        public override void Run()
        {
            Client.CurrentPlayer.UpdatePing(_request, _msec, _unk2);
        }
    }
}