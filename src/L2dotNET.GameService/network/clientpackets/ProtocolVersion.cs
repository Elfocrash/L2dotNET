using log4net;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class ProtocolVersion : PacketBase
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ProtocolVersion));
        private GameClient _client;
        public ProtocolVersion(Packet packet, GameClient client)
        {
            _client = client;
            _protocol = packet.ReadInt();
        }

        private int _protocol;

        public override void RunImpl()
        {
            if ((_protocol != 746) && (_protocol != 251))
            {
                Log.Error($"Protocol fail {_protocol}");
                _client.SendPacket(new KeyPacket(_client, 0));
                _client.Termination();
                return;
            }

            if (_protocol == -1)
            {
                Log.Info($"Ping received {_protocol}");
                _client.SendPacket(new KeyPacket(_client, 0));
                _client.Termination();
                return;
            }

            Log.Info($"Accepted {_protocol} client");

            _client.SendPacket(new KeyPacket(_client, 1));
            _client.Protocol = _protocol;
        }
    }
}