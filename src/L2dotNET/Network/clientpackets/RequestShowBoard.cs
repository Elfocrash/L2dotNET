using L2dotNET.Managers.bbs;

namespace L2dotNET.Network.clientpackets
{
    class RequestShowBoard : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _type;

        public RequestShowBoard(Packet packet, GameClient client)
        {
            _client = client;
            _type = packet.ReadInt();
        }

        public override void RunImpl()
        {
            BbsManager.Instance.RequestShow(_client.CurrentPlayer, _type);
        }
    }
}