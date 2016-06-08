using L2dotNET.GameService.Managers.BBS;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestShowBoard : GameServerNetworkRequest
    {
        private int type;

        public RequestShowBoard(GameClient client, byte[] data)
        {
            makeme(client, data);
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