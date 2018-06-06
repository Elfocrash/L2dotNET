using System;
using System.Threading.Tasks;
using L2dotNET.Logging.Abstraction;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets
{
    class ProtocolVersion : PacketBase
    {
        private static readonly ILog Log = LogProvider.GetCurrentClassLogger();

        private readonly GameClient _client;
        private readonly int _protocol;

        public ProtocolVersion(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _protocol = packet.ReadInt();
        }

        public override async Task RunImpl()
        {
            if ((_protocol != 746) && (_protocol != 251))
            {
                Log.Error($"Protocol fail {_protocol}");
                await _client.SendPacketAsync(new KeyPacket(_client, 0));
                _client.Termination();
                return;
            }

            if (_protocol == -1)
            {
                Log.Info($"Ping received {_protocol}");
                await _client.SendPacketAsync(new KeyPacket(_client, 0));
                _client.Termination();
                return;
            }

            Log.Info($"Accepted {_protocol} client");

            await _client.SendPacketAsync(new KeyPacket(_client, 1));
            _client.Protocol = _protocol;
        }
    }
}