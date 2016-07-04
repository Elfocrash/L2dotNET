using L2dotNET.GameService.Managers.BBS;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestShowBoard : GameServerNetworkRequest
    {
        private int _type;

        public RequestShowBoard(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        public override void Read()
        {
            _type = ReadD();
        }

        public override void Run()
        {
            BbsManager.Instance.RequestShow(Client.CurrentPlayer, _type);
        }
    }
}