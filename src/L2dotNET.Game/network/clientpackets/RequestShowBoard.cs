using L2dotNET.Game.managers.bbs;

namespace L2dotNET.Game.network.l2recv
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
            BBSManager.getInstance().RequestShow(Client.CurrentPlayer, type);
        }
    }
}
