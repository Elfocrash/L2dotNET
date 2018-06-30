using System;
using System.Threading.Tasks;
using NLog;


namespace L2dotNET.Network.clientpackets.PartyAPI
{
    class RequestPartyMatchList : PacketBase
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly GameClient _client;
        private readonly int _status;

        public RequestPartyMatchList(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _status = packet.ReadInt();
        }

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                Log.Info($"party {_status}");
            });
        }
    }
}