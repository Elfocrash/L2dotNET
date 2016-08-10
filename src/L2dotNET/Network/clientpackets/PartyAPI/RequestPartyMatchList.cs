using log4net;

namespace L2dotNET.Network.clientpackets.PartyAPI
{
    class RequestPartyMatchList : PacketBase
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RequestPartyMatchList));

        private readonly GameClient _client;
        private readonly int _status;

        public RequestPartyMatchList(Packet packet, GameClient client)
        {
            _client = client;
            _status = packet.ReadInt();
        }

        public override void RunImpl()
        {
            Log.Info($"party {_status}");
        }
    }
}