using log4net;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets
{
    class ProtocolVersion : PacketBase
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ProtocolVersion));

        private readonly GameClient _client;
        private readonly int _protocol;

        public ProtocolVersion(Packet packet, GameClient client)
        {
            _client = client;
            _protocol = packet.ReadInt();
        }

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