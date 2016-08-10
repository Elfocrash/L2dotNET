namespace L2dotNET.Network.clientpackets
{
    class NetPingResponse : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _request;
        private readonly int _msec;
        private readonly int _unk2;

        public NetPingResponse(Packet packet, GameClient client)
        {
            _client = client;
            _request = packet.ReadInt();
            _msec = packet.ReadInt();
            _unk2 = packet.ReadInt();
        }

        public override void RunImpl() { }
    }
}