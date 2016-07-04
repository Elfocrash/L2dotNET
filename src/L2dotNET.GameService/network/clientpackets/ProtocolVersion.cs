using log4net;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class ProtocolVersion : GameServerNetworkRequest
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ProtocolVersion));

        public ProtocolVersion(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        private int _protocol;

        public override void Read()
        {
            _protocol = ReadD();
        }

        public override void Run()
        {
            if ((_protocol != 746) && (_protocol != 251))
            {
                Log.Error($"Protocol fail {_protocol}");
                GetClient().SendPacket(new KeyPacket(GetClient(), 0));
                GetClient().Termination();
                return;
            }

            if (_protocol == -1)
            {
                Log.Info($"Ping received {_protocol}");
                GetClient().SendPacket(new KeyPacket(GetClient(), 0));
                GetClient().Termination();
                return;
            }

            Log.Info($"Accepted {_protocol} client");

            GetClient().SendPacket(new KeyPacket(GetClient(), 1));
            GetClient().Protocol = _protocol;
        }
    }
}