using L2dotNET.GameService.Managers.BBS;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets
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