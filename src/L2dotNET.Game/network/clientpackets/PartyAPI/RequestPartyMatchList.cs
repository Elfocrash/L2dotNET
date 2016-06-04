using log4net;

namespace L2dotNET.GameService.network.l2recv
{
    class RequestPartyMatchList : GameServerNetworkRequest
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RequestPartyMatchList));

        public RequestPartyMatchList(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }


        private int _status;
        public override void read()
        {
            _status = readD();
        }

        public override void run()
        {
            log.Info($"party { _status }");
            
        }
    }
}
