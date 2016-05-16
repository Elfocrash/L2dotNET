using L2dotNET.GameService.managers.bbs;

namespace L2dotNET.GameService.network.l2recv
{
    class RequestShowBoard : GameServerNetworkRequest
    {
        private int type;
        public RequestShowBoard(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        public override void read()
        {
            type = readD();
        }

        public override void run()
        {
            BBSManager.Instance.RequestShow(Client.CurrentPlayer, type);
        }
    }
}
