using log4net;

namespace L2dotNET.GameService.Network.Clientpackets.PartyAPI
{
    class RequestPartyMatchList : GameServerNetworkRequest
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RequestPartyMatchList));

        public RequestPartyMatchList(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        private int _status;

        public override void Read()
        {
            _status = ReadD();
        }

        public override void Run()
        {
            Log.Info($"party {_status}");
        }
    }
}