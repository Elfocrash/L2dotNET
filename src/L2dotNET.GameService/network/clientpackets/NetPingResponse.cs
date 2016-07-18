using L2dotNET.GameService.Config;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class NetPingResponse : PacketBase
    {
        private int _request;
        private int _msec;
        private int _unk2;
        private readonly GameClient _client;

        public NetPingResponse(Packet packet, GameClient client)
        {
            _client = client;
            _request = packet.ReadInt();
            _msec = packet.ReadInt();
            _unk2 = packet.ReadInt();
        }

        public override void RunImpl()
        {
            
        }
    }
}